using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class LogicExpression :Expression
    {
        public LogicExpression()
        {
            Operator = Operator.And;
        }
        public Expression Left { get; set; }
        public Expression Right { get; set; }
        public Operator Operator { get; set; }
        public string ReturnType
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
                desc.Add(new ExpressionDescriptor(this, "Left", "boolean"));
                desc.Add(new SelectionItemDescriptor(this, "Operator", new object[] { "And", "OR"},
                    new object[] { Operator.And, Operator.Or }));
                desc.Add(new TextItemDescriptor(this, " "));
                desc.Add(new ExpressionDescriptor(this, "Right", "boolean"));
                return desc;
            }
        }

        public string Type
        {
            get { return "AddExpression"; }
        }
        public bool IsClosing
        {
            get { return false; }
        }
        bool result;
        bool leftResult;
        int current;
        //execution  

        public ExecutionEnvironment StartCall(ExecutionEnvironment e)
        {
            result = false;
            leftResult = false;
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
                execution = Left;
                callback = Callback;
                return true;
            }
            if (current == 1)
            {
                execution = Right;
                callback = Callback;
                return false;
            }
            execution = null;
            callback = null;
            return false;
        }
        Nullable<DateTime> Callback(object value, object exception, ExecutionEnvironment e)
        {
            if (current == 0)
                leftResult = (bool)value;
            else if(current==1)
            {
                switch (Operator)
                {
                    case Operator.And:
                        result=leftResult&&(bool)value;
                        break;
                    case Operator.Or:
                        result=leftResult||(bool)value;
                        break;
                }
            }
            current++;
            return null;
        }
        public bool HandleException(object exception)
        {
            throw new NotImplementedException();
        }
    }
}
