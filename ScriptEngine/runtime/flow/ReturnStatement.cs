using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class ReturnStatement : Statement, Execution2
    {
        public ReturnStatement()
        {
        }

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
                desc.Add(new TextItemDescriptor(this, "Return "));
                return desc;
            }
        }
        public BlockDescriptor BlockDescriptor
        {
            get
            {
                return null;
            }
        }
        public string Type
        {
            get
            {
                return "ReturnStatement";
            }
        }
        public bool IsClosing
        {
            get { return true; }
        }
        //execution    

        public ExecutionEnvironment StartCall(ExecutionEnvironment e)
        {
            return e;
        }

        public Completion EndCall(ExecutionEnvironment e)
        {
            return Completion.Void;
        }

        public bool PopStack(out object execution, out ExecutionCallback callback, ExecutionEnvironment e)
        {
            execution = null;
            callback = null;
            return false;
        }
        Nullable<DateTime> Callback(object value, object exception, ExecutionEnvironment e)
        {
            return null;
        }
        public bool HandleException(object exception)
        {
            throw new NotImplementedException();
        }
    }
}
