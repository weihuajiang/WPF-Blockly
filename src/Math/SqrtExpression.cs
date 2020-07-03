using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class SqrtExpression : MathFunctionBase
    {
        public SqrtExpression()
        {
            MathFunction = "Sqrt";
            Args.Add(null);
            FunctionDisplay = "sqrt";
        }
    }
}
