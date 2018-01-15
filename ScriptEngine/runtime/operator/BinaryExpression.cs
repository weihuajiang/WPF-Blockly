using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class BinaryExpression : Expression
    {
        public Expression Left { get; set; }
        public Operator Operator { get; set; }
        public Expression Right { get; set; }
        public BinaryExpression()
        {
            Operator = Operator.Add;
        }

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
                desc.Add(new ExpressionDescriptor(this, "Left", "number") );
                desc.Add(new TextItemDescriptor(this, " "));
                desc.Add(new SelectionItemDescriptor(this, "Operator", new object[] { "+", "-", "*", "/" },
                    new object[] { Operator.Add, Operator.Minus, Operator.Mulitiply, Operator.Divide }));
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
        double result;
        int current = 0;

        public ExecutionEnvironment StartCall(ExecutionEnvironment e)
        {
            result = 0;
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
        double GetValue(object value)
        {
            if (value is double)
                return (double)value;
            else if ((value + "") == "")
                return 0;
            return double.NaN;
        }
        Nullable<DateTime> Callback(object value, object exception, ExecutionEnvironment e)
        {
            if(current==0)
            {
                result = GetValue(value);
            }
            if (current == 1)
            {
                double r = GetValue(value);
                switch (Operator)
                {
                    case Operator.Add:
                        result+=r;
                        break;
                    case Operator.Minus:
                        result=result-r;
                        break;
                    case Operator.Mulitiply:
                        result = result * r;
                        break;
                    case Operator.Divide:
                        if (r == 0)
                            result = result<0?double.NegativeInfinity:double.PositiveInfinity;
                        else
                            result = result / r;
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
    public enum Operator
    {
        Add,
        Minus,
        Mulitiply,
        Divide,
        Equal,
        Great,
        Less,
        NotEqual,
        GreatOrEqual,
        LessOrEqual,
        And,
        Or
    }
}
