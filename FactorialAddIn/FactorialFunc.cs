using MadCalc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactorialAddIn
{
    public class FactorialFunc : ICalcFunc
    {
        public double Calculate(double[] ops)
        {
            return Factorial((int)ops[0]);
        }

        private int Factorial(int n)
        {
            int result = 1;
            for (int i = 2; i <= n; i++)
            {
                result *= i;
            }
            return result;
        }
    }
}
