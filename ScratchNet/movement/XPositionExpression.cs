using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class XPositionExpression : Expression
    {
        public string ReturnType
        {
            get { return "number"; }
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
                desc.Add(new TextItemDescriptor(this, "X"));
                return desc;
            }
        }

        public string Type
        {
            get { return "XPositionExpression"; }
        }
        public bool IsClosing
        {
            get { return false; }
        }
        //execution  

        public ExecutionEnvironment StartCall(ExecutionEnvironment e)
        {
            return e;
        }
        public Completion EndCall(ExecutionEnvironment _env)
        {
            double result = ((_env.This as Instance).Class as Sprite).X;
            return new Completion(result);
        }

        public bool PopStack(out object execution, out ExecutionCallback callback, ExecutionEnvironment e)
        {
            execution = null;
            callback = Callback;
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
