using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadCalc
{
    public interface ICalcFunc
    {
        double Calculate(double[] ops);
    }
}
