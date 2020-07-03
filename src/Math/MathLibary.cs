using ScratchNet.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class MathLibary : Library
    {
        public MathLibary()
        {
            Name = "Math";
            Title = Language.Title;
            DefaultColor = "#f2a66f";
            Description = Language.Description;
            CommandGroup stack = new CommandGroup(Language.MathCategory,"");
            stack.Add(new Command("random", Language.RandomDescription, true, new RandomExpression()));
            stack.Add(new Command("max", Language.MaxDescription, true, new MaxExpression()));
            stack.Add(new Command("min", Language.MinDescription, true, new MinExpression()));
            stack.Add(new Command("abs", Language.AbsDescription, true, new AbsExpression()));
            stack.Add(new Command("sqrt", "sqrt", true, new SqrtExpression()));
            stack.Add(new Command("π", Language.PiDescription, true, new PIExpression()));
            stack.Add(new Command("e", Language.EDescription, true, new EExpression()));
            stack.Add(new Command("pow", "specified number raised to the specified power", true, new PowExpression()));
            stack.Add(new Command("ceiling", "smallest integral value that is greater than or equal to the specified", true, new CeilingExpression()));
            stack.Add(new Command("floor", "largest integer less than or equal to the specified double-precision", true, new FloorExpression()));
            stack.Add(new Command("log", "natural (base e) logarithm of a specified number", true, new LogExpression()));
            stack.Add(new Command("log10", "the base 10 logarithm of a specified number", true, new Log10Expression()));
            stack.Add(new Command("log", "logarithm of a specified number in a specified base", true, new LogExExpression()));
            CommandGroup triangle = new CommandGroup(Language.TriangleCategory, "");
            triangle.Add(new Command("sin", Language.SinDescription, true, new SinExpression()));
            triangle.Add(new Command("cos", Language.CosDescription, true, new CosExpression()));
            triangle.Add(new Command("tan", Language.TanDescription, true, new TanExpression()));
            triangle.Add(new Command("ctg", Language.CtgDescription, true, new CtgExpression()));
            triangle.Add(new Command("asin", Language.ASinDescription, true, new ASinExpression()));
            triangle.Add(new Command("acos", Language.AcosDescription, true, new ACosExpression()));
            triangle.Add(new Command("atan", Language.AtanDescription, true, new ATanExpression()));
            triangle.Add(new Command("actg", Language.ACtgDescription, true, new ACtgExpression()));
            Add(stack);
            Add(triangle);
        }
    }
}
