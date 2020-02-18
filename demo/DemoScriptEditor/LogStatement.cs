using ScratchNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoScriptEditor
{
    public delegate void OutWriteLine(object obj, params object[] param);
    public class LogStatement : Statement
    {
        public LogStatement()
        {
            Message = new Literal() { Raw = "Log Text" };
        }
        public Expression Message { get; set; }
        public string ReturnType
        {
            get { return "void"; }
        }
        public Completion Execute(ExecutionEnvironment enviroment)
        {
            if (Message == null)
                return Completion.Void;
            var c = Message.Execute(enviroment);
            if (c.Type != CompletionType.Value)
                return c;
            var value = c.ReturnValue;
            //Console.WriteLine(DateTime.Now + ":" + DateTime.Now.Millisecond + " " + value);
            if (WriteLine != null)
            {
                WriteLine(value);
            }
            return Completion.Void;
        }

        public Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "Console.WriteLine("));
                desc.Add(new ExpressionDescriptor(this, "Message", "any") { IsOnlyNumberAllowed = false });
                desc.Add(new TextItemDescriptor(this, ")"));
                return desc;
            }
        }
        public string Type
        {
            get
            {
                return "MoveStatement";
            }
        }


        public BlockDescriptor BlockDescriptor
        {
            get { return null; }
        }


        public bool IsClosing
        {
            get { return false; }
        }

        public static OutWriteLine WriteLine { get; set; }
        
    }
}
