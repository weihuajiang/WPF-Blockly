using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class LogStatement : Statement, Execution2
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
            return Completion.Void;
        }

        public Descriptor Descriptor
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

        //execution
        object logValue = null;

        public ExecutionEnvironment StartCall(ExecutionEnvironment e)
        {
            return e;
        }

        public Completion EndCall(ExecutionEnvironment e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(DateTime.Now+":"+DateTime.Now.Millisecond+" "+ logValue);
            Console.ForegroundColor = ConsoleColor.White;
            return Completion.Void;
        }

        public bool PopStack(out object execution, out ExecutionCallback callback, ExecutionEnvironment e)
        {
            execution = Message;
            callback = Callback;
            return false;
        }
        Nullable<DateTime> Callback(object value, object exception, ExecutionEnvironment e)
        {
            logValue = value;
            return null;
        }
        public bool HandleException(object exception)
        {
            throw new NotImplementedException();
        }
    }
}
