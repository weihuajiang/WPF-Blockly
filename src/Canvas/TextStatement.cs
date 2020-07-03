using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class TextStatement : Statement
    {
        public Expression Text { get; set; }

        public override string ReturnType => "void";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "text("));
                desc.Add(new ExpressionDescriptor(this, "Text", "any"));
                desc.Add(new TextItemDescriptor(this, ");"));
                return desc;
            }
        }

        public override string Type => "TextStatement";

        public override BlockDescriptor BlockDescriptor => null;

        public override bool IsClosing => false;

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Text == null)
                return Completion.Exception(Properties.Language.NullException, this);
            var c = Text.Execute(enviroment);
            if (!c.IsValue)
                return c;
            string d = c.ReturnValue+"";
            DrawWindow wnd = CanvasEnvironment.GetCanvas(enviroment);
            wnd.Text(d);
            return Completion.Void;
        }
    }
}
