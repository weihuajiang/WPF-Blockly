using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class CompareExpression : Expression, Execution2
    {
        public Expression Left { get; set; }
        public Operator Operator { get; set; }
        public Expression Right { get; set; }

        public CompareExpression()
        {
            Operator = Operator.Equal;
        }

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
                desc.Add(new ExpressionDescriptor(this, "Left", "number"));
                desc.Add(new TextItemDescriptor(this, " "));
                desc.Add(new SelectionItemDescriptor(this, "Operator",
                    new object[]{"=","!=",">",">=","<","<="},
                    new object[]{Operator.Equal, Operator.NotEqual, Operator.Great, Operator.GreatOrEqual, Operator.Less, Operator.LessOrEqual}));

                desc.Add(new TextItemDescriptor(this, " ")); 
                desc.Add(new ExpressionDescriptor(this, "Right", "number"));
                return desc;
            }
        }

        public string Type
        {
            get { return "BinaryExpression"; }
        }
        public bool IsClosing
        {
            get { return false; }
        }

        //execution
        private bool result;
        private double leftValue;
        private int current = 0;

        public ExecutionEnvironment StartCall(ExecutionEnvironment e)
        {
            result = false;
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
            {
                leftValue = double.Parse(value + "");
            }
            if (current == 1)
            {
                double r = double.Parse(value + "");
                switch (Operator)
                {
                    case Operator.Equal:
                        result = leftValue == r;
                        break;
                    case Operator.NotEqual:
                        result = leftValue != r;
                        break;
                    case Operator.Great:
                        result = leftValue > r;
                        break;
                    case Operator.GreatOrEqual:
                        result = leftValue >= r;
                        break;
                    case Operator.Less:
                        result = leftValue < r;
                        break;
                    case Operator.LessOrEqual:
                        result = leftValue <= r;
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
