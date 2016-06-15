using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MadCalc
{
    public class Calculator
    {
        public double Calculate(string operation, string operants)
        {
            double[] ops = operants.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(s => double.Parse(s)).ToArray();
            switch (operation)
            {
                case "+":
                    return Add(ops[0], ops[1]);
                case "-":
                    return Substract(ops[0], ops[1]);
                case "*":
                    return Multiple(ops[0], ops[1]);
                case "/":
                    return Divide(ops[0], ops[1]);
                default:
                    if (_reflectionAddIns != null)
                    {
                        string method = _reflectionAddIns[operation];
                        if (method != null)
                        {
                            return (double)ReflectionUtilities.Execute(method, ops.Cast<object>().ToArray());
                        }
                    }
                    if (_dllAddIns != null)
                    {
                        string className = _dllAddIns[operation];
                        if (className != null)
                        {
                            //string[] parts = className.Split(',');
                            //Assembly a = Assembly.Load(parts[1]);
                            //Type type = a.GetType(parts[0]);
                            Type type = Type.GetType(className, true);
                            ICalcFunc cf = (ICalcFunc)Activator.CreateInstance(type);
                            return cf.Calculate(ops);
                        }
                    }
                    if (_scriptAddIns != null)
                    {
                        string script = _scriptAddIns[operation];
                        if (script != null)
                        {
                            MethodInfo mi = null;
                            _scriptCodes.TryGetValue(operation, out mi);
                            if (mi == null)
                            {
                                mi = RoslynUtilities.GenerateMethod(script);
                                _scriptCodes[operation] = mi;
                            }
                            return (double)mi.Invoke(null, ops.Cast<object>().ToArray());
                        }
                    }
                    throw new NotImplementedException(operation + " not implemented.");
            }
        }

        #region Hard-coded methods
        public double Add(double op1, double op2)
        {
            return op1 + op2;
        }

        public double Substract(double op1, double op2)
        {
            return op1 - op2;
        }

        public double Multiple(double op1, double op2)
        {
            return op1 * op2;
        }

        public double Divide(double op1, double op2)
        {
            return op1 / op2;
        }
        #endregion

        #region Add-Ons
        NameValueCollection _reflectionAddIns;
        NameValueCollection _dllAddIns;
        NameValueCollection _scriptAddIns;
        IDictionary<string, MethodInfo> _scriptCodes;
        
        public Calculator()
        {
            _reflectionAddIns = ConfigurationManager.GetSection("ReflectionAddIns") as NameValueCollection;
            _dllAddIns = ConfigurationManager.GetSection("DLLAddIns") as NameValueCollection;
            _scriptAddIns = ConfigurationManager.GetSection("ScriptAddIns") as NameValueCollection;
            _scriptCodes = new Dictionary<string, MethodInfo>();
        }

        public IEnumerable<string> AddIns
        {
            get
            {
                IEnumerable<string> addIns = Enumerable.Empty<string>();
                if (_reflectionAddIns != null)
                {
                    addIns = addIns.Concat(Enumerable.Cast<string>(_reflectionAddIns.Keys));
                }
                if (_dllAddIns != null)
                {
                    addIns = addIns.Concat(Enumerable.Cast<string>(_dllAddIns.Keys));
                }
                if (_scriptAddIns != null)
                {
                    addIns = addIns.Concat(Enumerable.Cast<string>(_scriptAddIns.Keys));
                }
                return addIns;
            }
        }
        #endregion
    }
}
