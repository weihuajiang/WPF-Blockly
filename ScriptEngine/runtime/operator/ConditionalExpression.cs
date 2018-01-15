using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class ConditionalExpression: Expression
    {
        public Expression Test { get; set; }
        public Expression Consequent { get; set; }
        public Expression Alternate { get; set; }
        public string ReturnType
        {
            get { return "number|string|boolean"; }
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
                desc.Add(new ExpressionDescriptor(this, "Test", "boolean"));
                desc.Add(new TextItemDescriptor(this, " ? "));
                desc.Add(new ExpressionDescriptor(this, "Consequent", "number|string|boolean"));
                desc.Add(new TextItemDescriptor(this, " : "));
                desc.Add(new ExpressionDescriptor(this, "Alternate", "number|string|boolean"));
                return desc;
            }
        }

        public string Type
        {
            get { return "ConditionalExpression"; }
        }
        public bool IsClosing
        {
            get { return false; }
        }
        //execution  
        bool testResult;
        object result;
        int current = 0;

        public ExecutionEnvironment StartCall(ExecutionEnvironment e)
        {
            testResult = false;
            result = null;
            current = 0;
            return e;
        }

        public Completion EndCall(ExecutionEnvironment e)
        {
            return new Completion(result);
        }

        public bool PopStack(out object execution, out ExecutionCallback callback, ExecutionEnvironment e)
        {
            if (current == 0)
            {
                execution = Test;
                callback = Callback;
                return true;
            }
            else
            {
                if (testResult)
                    execution = Consequent;
                else
                    execution = Alternate;
            }
            callback = Callback;
            return false;
        }
        Nullable<DateTime> Callback(object value, object exception, ExecutionEnvironment e)
        {
            if (current == 0)
                testResult = (bool)value;
            else
                result = value;
            current++;
            return null;
        }
        public bool HandleException(object exception)
        {
            throw new NotImplementedException();
        }
    }
}
