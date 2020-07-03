using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class PrintStatement : ConsoleBaseStatement
    {
        public PrintStatement()
        {
            Message = new Literal() { Raw = "\"Hello World!\"" };
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
            var value = c.ReturnValue;
            ConsoleWindow wnd = GetConsole(enviroment);
            wnd.Write(value);
            return Completion.Void;
        }
        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "print("));
                desc.Add(new ExpressionDescriptor(this, "Message", "any") { IsOnlyNumberAllowed = false });
                desc.Add(new TextItemDescriptor(this, ");"));
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
