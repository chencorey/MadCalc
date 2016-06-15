using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.IO;

namespace MadCalc
{
    public class RoslynUtilities
    {
        const string CODE_TEMPLATE = @"
using System; 
public class @class 
{ 
    public static @function 
}
";

        public static MethodInfo GenerateMethod(string function)
        {
            string className = string.Format("P{0}", Guid.NewGuid().ToString().Replace("-", string.Empty));
            string code = CODE_TEMPLATE
                .Replace("@class", className)
                .Replace("@function", function);

            var tree = CSharpSyntaxTree.ParseText(code);
            var compilaton = CSharpCompilation.Create(className)
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddSyntaxTrees(tree);
            var check = compilaton.GetDiagnostics();
            if (check.Count() > 0)
            {
                throw new Exception(check[0].ToString());
            }

            Assembly assembly;
            using (var ms = new MemoryStream())
            {
                compilaton.Emit(ms);
                assembly = Assembly.Load(ms.GetBuffer());
            }

            Type myType = assembly.GetType(className);
            return myType.GetMethods()[0];
        }
    }
}
