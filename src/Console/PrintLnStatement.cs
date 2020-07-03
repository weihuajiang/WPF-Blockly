using ScratchNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ScratchNet
{
    public class PrintLnStatement : ConsoleBaseStatement
    {
        public PrintLnStatement()
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
            ConsoleWindow console = GetConsole(enviroment);
            console.Write(value+"\n");
            return Completion.Void;
        }
        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "println("));
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
