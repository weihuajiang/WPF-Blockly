using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class AssignmentStatement : Statement
    {
        public AssignmentStatement()
        {
            
        }
        public Expression Left { get; set; }
        public Expression Right { get; set; }
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
            get {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "Set "));
                desc.Add(new VariableDescriptor(this, "Left"));
                desc.Add(new TextItemDescriptor(this, " to "));
                desc.Add(new ExpressionDescriptor(this, "Right", "number|string|boolean"));
                return desc;
            }
        }

        public string Type
        {
            get { return "VariableAssignmentExpression"; }
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
        object result;
        int current = 0;
        string variable;

        public ExecutionEnvironment StartCall(ExecutionEnvironment e)
        {
            result = null;
            current = 0;
            variable = null;
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
                execution = Right;
                callback = Callback;
                return false;
            }
            execution = null;
            callback = null;
            return false;
        }
        Nullable<DateTime> Callback(object value, object exception, ExecutionEnvironment _env)
        {
            variable = (Left as Identifier).Variable;
            result = value;
            _env.SetValue(variable, value);
            current++;
            return null;
        }
        public bool HandleException(object exception)
        {
            throw new NotImplementedException();
        }
    }
}
