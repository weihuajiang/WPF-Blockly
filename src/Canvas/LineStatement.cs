using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class LineStatement : Statement
    {
        public Expression Step { get; set; }

        public override string ReturnType => "void";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "line("));
                desc.Add(new ExpressionDescriptor(this, "Step", "number") { IsOnlyNumberAllowed = true });
                desc.Add(new TextItemDescriptor(this, ");"));
                return desc;
            }
        }

        public override string Type => "LineStatement";

        public override BlockDescriptor BlockDescriptor => null;

        public override bool IsClosing => false;

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Step == null)
                return Completion.Exception(Properties.Language.NullException, this);
            var c = Step.Execute(enviroment);
            if (!c.IsValue)
                return c;
            if (!TypeConverters.IsNumber(c.ReturnValue))
                return Completion.Exception(Properties.Language.ValueNotNumberException, Step);
            double d = TypeConverters.GetValue<double>(c.ReturnValue);
            DrawWindow wnd = CanvasEnvironment.GetCanvas(enviroment);
            wnd.Line(d);
            return Completion.Void;
        }
    }
    public class LineToStatement : Statement
    {
        public Expression X { get; set; }
        public Expression Y { get; set; }

        public override string ReturnType => "void";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "lineTo("));
                desc.Add(new ExpressionDescriptor(this, "X", "number") { IsOnlyNumberAllowed = true });
                desc.Add(new TextItemDescriptor(this, ", "));
                desc.Add(new ExpressionDescriptor(this, "Y", "number") { IsOnlyNumberAllowed = true });
                desc.Add(new TextItemDescriptor(this, ");"));
                return desc;
            }
        }

        public override string Type => "LineStatement2";

        public override BlockDescriptor BlockDescriptor => null;

        public override bool IsClosing => false;

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (X == null || Y==null)
                return Completion.Exception(Properties.Language.NullException, this);
            var c = X.Execute(enviroment);
            if (!c.IsValue)
                return c;
            if (!TypeConverters.IsNumber(c.ReturnValue))
                return Completion.Exception(Properties.Language.ValueNotNumberException, X);
            double d = TypeConverters.GetValue<double>(c.ReturnValue);

            var e = X.Execute(enviroment);
            if (!e.IsValue)
                return e;
            if (!TypeConverters.IsNumber(e.ReturnValue))
                return Completion.Exception(Properties.Language.ValueNotNumberException, Y);
            double f = TypeConverters.GetValue<double>(e.ReturnValue);

            DrawWindow wnd = CanvasEnvironment.GetCanvas(enviroment);
            wnd.LineTo(d, f);
            return Completion.Void;
        }
    }
}
