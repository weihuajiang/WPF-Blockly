using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class LogStatement : Statement
    {
        public LogStatement()
        {
            Message = new Literal() { Raw = "Log Text" };
        }
        public Expression Message { get; set; }
        public override string ReturnType
        {
            get { return "void"; }
        }
        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Message == null)
                return Completion.Void;
            var c = Message.Execute(enviroment);
            if (c.Type != CompletionType.Value)
                return c;
            object logValue = c.ReturnValue;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(DateTime.Now + ":" + DateTime.Now.Millisecond + " " + logValue);
            Console.ForegroundColor = ConsoleColor.White;
            return Completion.Void;
        }

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, Localize.GetString("cs_LoseTo")));
                desc.Add(new ExpressionDescriptor(this, "Message", "string|number|boolean"){IsOnlyNumberAllowed=false});
                desc.Add(new TextItemDescriptor(this, Localize.GetString("cs_ToLogWindow")));
                return desc;
            }
        }
        public override string Type
        {
            get
            {
                return "MoveStatement";
            }
        }


        public override BlockDescriptor BlockDescriptor
        {
            get { return null; }
        }


        public override bool IsClosing
        {
            get { return false; }
        }

    }
}
