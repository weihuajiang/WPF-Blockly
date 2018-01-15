using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class NotExpression : Expression
    {
        public Expression Argument { get; set; }
        public  string ReturnType
        {
            get { return "boolean"; }
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
                desc.Add(new TextItemDescriptor(this, "not "));
                desc.Add(new ExpressionDescriptor(this, "Argument", "boolean"));
                return desc;
            }
        }
        bool result;
        public string Type
        {
            get { return "UnaryExpression"; }
        }
        public bool IsClosing
        {
            get { return false; }
        }//execution  

        public ExecutionEnvironment StartCall(ExecutionEnvironment e)
        {
            result = false;
            return e;
        }

        public Completion EndCall(ExecutionEnvironment e)
        {
            return new Completion(result);
        }

        public bool PopStack(out object execution, out ExecutionCallback callback, ExecutionEnvironment e)
        {
            execution = Argument;
            callback = Callback;
            return false;
        }
        Nullable<DateTime> Callback(object value, object exception, ExecutionEnvironment e)
        {
            result = !(bool)value;
            return null;
        }
        public bool HandleException(object exception)
        {
            throw new NotImplementedException();
        }
    }
}
