using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class AbsExpression : MathFunctionBase
    {
        public AbsExpression()
        {
            MathFunction = "Abs";
            Args.Add(null);
            FunctionDisplay = "abs";
        }
    }
    public class PowExpression : MathFunctionBase
    {
        public PowExpression()
        {
            MathFunction = "Pow";
            Args.Add(null);
            Args.Add(null);
            FunctionDisplay = "pow";
        }
    }
    public class Log10Expression : MathFunctionBase
    {
        public Log10Expression()
        {
            MathFunction = "Log10";
            Args.Add(null);
            FunctionDisplay = "log10";
        }
    }
    public class LogExpression : MathFunctionBase
    {
        public LogExpression()
        {
            MathFunction = "Log";
            Args.Add(null);
            FunctionDisplay = "log";
        }
    }
    public class LogExExpression : MathFunctionBase
    {
        public LogExExpression()
        {
            MathFunction = "Log";
            Args.Add(new Literal("value"));
            Args.Add(new Literal("base"));
            FunctionDisplay = "log";
        }
    }
    public class FloorExpression : MathFunctionBase
    {
        public FloorExpression()
        {
            MathFunction = "Floor";
            Args.Add(null);
            FunctionDisplay = "floor";
        }
    }
    public class ExpExpression : MathFunctionBase
    {
        public ExpExpression()
        {
            MathFunction = "Exp";
            Args.Add(null);
            FunctionDisplay = "Exp";
        }
    }
    public class RoundExpression : MathFunctionBase
    {
        public RoundExpression()
        {
            MathFunction = "Round";
            Args.Add(null);
            FunctionDisplay = "round";
        }
    }
    public class CeilingExpression : MathFunctionBase
    {
        public CeilingExpression()
        {
            MathFunction = "Ceiling";
            Args.Add(null);
            FunctionDisplay = "ceiling";
        }
    }
}
