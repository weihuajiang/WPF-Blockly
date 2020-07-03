using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class MinExpression : MathFunctionBase
    {
        public MinExpression()
        {
            MathFunction = "Min";
            Args.Add(null);
            Args.Add(null);
            FunctionDisplay = "min";
        }
    }
}
