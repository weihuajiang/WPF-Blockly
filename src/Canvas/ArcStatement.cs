using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class ArcStatement : Statement
    {
        public Expression Step { get; set; } = new Literal("step");
        public Expression Angle { get; set; } = new Literal("angle");
        public Expression XRadius { get; set; } = new Literal("xRadius");
        public Expression YRadius { get; set; } = new Literal("yRadius");

        public override string ReturnType => "void";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "arc("));
                desc.Add(new ExpressionDescriptor(this, "Step", "number") { IsOnlyNumberAllowed = true });
                desc.Add(new TextItemDescriptor(this, ", "));
                desc.Add(new ExpressionDescriptor(this, "Angle", "number") { IsOnlyNumberAllowed = true });
                desc.Add(new TextItemDescriptor(this, ", "));
                desc.Add(new ExpressionDescriptor(this, "XRadius", "number") { IsOnlyNumberAllowed = true });
                desc.Add(new TextItemDescriptor(this, ", "));
                desc.Add(new ExpressionDescriptor(this, "YRadius", "number") { IsOnlyNumberAllowed = true });
                desc.Add(new TextItemDescriptor(this, ");"));
                return desc;
            }
        }

        public override string Type => "ArcStatement";

        public override BlockDescriptor BlockDescriptor => null;

        public override bool IsClosing => false;

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Step == null || Angle==null || XRadius==null || YRadius==null)
                return Completion.Exception(Properties.Language.NullException, this);

            var c = Step.Execute(enviroment);
            if (!c.IsValue)
                return c;
            if (!TypeConverters.IsNumber(c.ReturnValue))
                return Completion.Exception(Properties.Language.ValueNotNumberException, Step);
            double step = TypeConverters.GetValue<double>(c.ReturnValue);

            var d = Angle.Execute(enviroment);
            if (!d.IsValue)
                return d;
            if (!TypeConverters.IsNumber(d.ReturnValue))
                return Completion.Exception(Properties.Language.ValueNotNumberException, Angle);
            double angle = TypeConverters.GetValue<double>(d.ReturnValue);

            var e = XRadius.Execute(enviroment);
            if (!e.IsValue)
                return e;
            if (!TypeConverters.IsNumber(e.ReturnValue))
                return Completion.Exception(Properties.Language.ValueNotNumberException, XRadius);
            double x = TypeConverters.GetValue<double>(e.ReturnValue);

            var f = YRadius.Execute(enviroment);
            if (!f.IsValue)
                return f;
            if (!TypeConverters.IsNumber(f.ReturnValue))
                return Completion.Exception(Properties.Language.ValueNotNumberException, YRadius);
            double y = TypeConverters.GetValue<double>(f.ReturnValue);
            DrawWindow wnd = CanvasEnvironment.GetCanvas(enviroment);
            wnd.Arc(step, angle, x, y);
            return Completion.Void;
        }
    }
}
