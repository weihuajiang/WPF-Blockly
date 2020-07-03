using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class MaxExpression : MathFunctionBase
    {
        public MaxExpression()
        {
            MathFunction = "Max";
            Args.Add(null);
            Args.Add(null);
            FunctionDisplay = "max";
        }
    }
}
