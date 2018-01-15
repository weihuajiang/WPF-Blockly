using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class Literal : Expression
    {
        public string ReturnType
        {
            get { return ""; }
        }

        public Completion Execute(ExecutionEnvironment enviroment)
        {
            throw new NotImplementedException();
        }

        public Descriptor Descriptor
        {
            get { throw new NotImplementedException(); }
        }

        public string Type
        {
            get { return "Literal"; }
        }
        public object Value
        {
            get
            {
                try
                {
                    return double.Parse(Raw);
                }
                catch (Exception e) {
                }

                return Raw;
            }
        }
        public string Raw
        {
            get;
            set;
        }

        //execution


        public ExecutionEnvironment StartCall(ExecutionEnvironment e)
        {
            return e;
        }

        public Completion EndCall(ExecutionEnvironment e)
        {
            return new Completion(Value);
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
