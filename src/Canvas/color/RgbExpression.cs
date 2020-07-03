using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ScratchNet
{
    public class RgbExpression : Expression
    {
        public Expression R { get; set; } = new Literal("red");
        public Expression G { get; set; } = new Literal("green");
        public Expression B { get; set; } = new Literal("blue");

        public override string ReturnType => "color";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "rgb("));
                desc.Add(new ExpressionDescriptor(this, "R", "number") { IsOnlyNumberAllowed = true });
                desc.Add(new TextItemDescriptor(this, ", "));
                desc.Add(new ExpressionDescriptor(this, "G", "number") { IsOnlyNumberAllowed = true });
                desc.Add(new TextItemDescriptor(this, ", "));
                desc.Add(new ExpressionDescriptor(this, "B", "number") { IsOnlyNumberAllowed = true });
                desc.Add(new TextItemDescriptor(this, ")"));
                return desc;
            }
        }

        public override string Type => "ColorExpression";

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (R == null || G == null || B == null)
                return Completion.Exception(Properties.Language.NullException, this);
            var r = R.Execute(enviroment);
            if (!r.IsValue)
                return r;
            if (!TypeConverters.IsNumber(r.ReturnValue))
                return Completion.Exception(Properties.Language.ValueNotNumberException, R);
            int ri = TypeConverters.GetValue<int>(r.ReturnValue);
            if (ri < 0 || ri > 255)
                return Completion.Exception(Properties.Language.ColorValueOutRangeException, R);
            var g = R.Execute(enviroment);
            if (!g.IsValue)
                return g;
            if (!TypeConverters.IsNumber(g.ReturnValue))
                return Completion.Exception(Properties.Language.ValueNotNumberException, G);
            int gi = TypeConverters.GetValue<int>(g.ReturnValue);
            if (gi < 0 || gi > 255)
                return Completion.Exception(Properties.Language.ColorValueOutRangeException, G);
            var b = R.Execute(enviroment);
            if (!b.IsValue)
                return b;
            if (!TypeConverters.IsNumber(b.ReturnValue))
                return Completion.Exception(Properties.Language.ValueNotNumberException, B);
            int bi = TypeConverters.GetValue<int>(b.ReturnValue);
            if (bi < 0 || bi > 255)
                return Completion.Exception(Properties.Language.ColorValueOutRangeException, B);
            return new Completion(Color.FromRgb((byte)ri, (byte)gi, (byte)bi));
        }
    }
}
