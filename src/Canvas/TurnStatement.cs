using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class TurnStatement : Statement
    {
        public Expression Step { get; set; }

        public override string ReturnType => "void";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "turn("));
                desc.Add(new ExpressionDescriptor(this, "Step", "number") { IsOnlyNumberAllowed = true });
                desc.Add(new TextItemDescriptor(this, ");"));
                return desc;
            }
        }

        public override string Type => "TurnStatement";

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
            wnd.Turn(d);
            return Completion.Void;
        }
    }
}
