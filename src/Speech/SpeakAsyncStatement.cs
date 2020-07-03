using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class SpeekAsyncStatement : Statement
    {
        public Expression Text { get; set; }
        public override string ReturnType => "void";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "speakAsync("));
                desc.Add(new ExpressionDescriptor(this, "Text", "any"));
                desc.Add(new TextItemDescriptor(this, ");"));
                return desc;
            }
        }

        public override string Type => "SpeekStatement";

        public override BlockDescriptor BlockDescriptor => null;

        public override bool IsClosing => false;

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Text == null)
                return Completion.Void;
            var c = Text.Execute(enviroment);
            if (!c.IsValue)
                return c;
            string t = c.ReturnValue + "";
            SpeachBase.GetSpeech(enviroment).SpeakAsync(t);
            return Completion.Void;
        }
    }
}
