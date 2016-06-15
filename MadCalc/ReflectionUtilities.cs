using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadCalc
{
    public class ReflectionUtilities
    {
        public static object Execute(string method, object[] args)
        {
            int x = method.LastIndexOf(".");
            string typeName = method.Substring(0, x);
            method = method.Substring(x + 1);
            Type type = Type.GetType(typeName);
            var mi = type.GetMethod(method);
            return mi.Invoke(null, args);
        }
    }
}
