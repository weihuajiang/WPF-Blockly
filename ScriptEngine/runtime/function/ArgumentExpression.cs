using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class ArgumentExpression: Expression
    {
        public String Variable { get; set; }
        public string VarType { get; set; }
        public string ReturnType
        {
            get { return VarType; }
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
                desc.Add(new TextItemDescriptor(this, Variable));
                return desc;
            }
        }

        public string Type
        {
            get { return "ArgumentExpression"; }
        }
        public bool IsClosing
        {
            get { return false; }
        }//execution  


        public ExecutionEnvironment StartCall(ExecutionEnvironment e)
        {
            return e;
        }

        public Completion EndCall(ExecutionEnvironment _env)
        {
            object result = _env.GetValue(Variable);
            return new Completion(result);
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
