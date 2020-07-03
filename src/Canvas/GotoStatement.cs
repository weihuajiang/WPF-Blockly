using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class GotoStatement : Statement
    {
        public Expression X { get; set; }
        public Expression Y { get; set; }

        public override string ReturnType => "void";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "goto("));
                desc.Add(new ExpressionDescriptor(this, "X", "number") { IsOnlyNumberAllowed = true });
                desc.Add(new TextItemDescriptor(this, ", "));
                desc.Add(new ExpressionDescriptor(this, "Y", "number") { IsOnlyNumberAllowed = true });
                desc.Add(new TextItemDescriptor(this, ");"));
                return desc;
            }
        }

        public override string Type => "GotoStatement";

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
            double x = TypeConverters.GetValue<double>(c.ReturnValue);
            var d = Y.Execute(enviroment);
            if (!d.IsValue)
                return d;
            if (!TypeConverters.IsNumber(d.ReturnValue))
                return Completion.Exception(Properties.Language.ValueNotNumberException, Y);
            double y = TypeConverters.GetValue<double>(d.ReturnValue);
            DrawWindow wnd = CanvasEnvironment.GetCanvas(enviroment);
            wnd.Goto(x, y);
            return Completion.Void;
        }
    }
}
