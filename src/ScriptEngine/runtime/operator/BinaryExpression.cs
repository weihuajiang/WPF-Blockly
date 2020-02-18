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
            if (Left == null || Right == null)
                return new Completion("Null Exception", CompletionType.Exception);
            var left = Left.Execute(enviroment);
            if (left.Type != CompletionType.Value)
                return left;
            var right = Right.Execute(enviroment);

            if (right.Type != CompletionType.Value)
                return right;
            Type T = TypeConverters.GetNumberTypes(left.ReturnValue, right.ReturnValue);
            if (T == null)
                return Completion.Exception("Only nuber can accepted", this);
            if (T.Equals(typeof(int)))
            {
                try
                {
                    var l = TypeConverters.GetValue<int>(left.ReturnValue);
                    var r = TypeConverters.GetValue<int>(right.ReturnValue);
                    switch (Operator)
                    {
                        case Operator.Add:
                            return new Completion(l + r);
                        case Operator.Minus:
                            return new Completion(l - r);
                        case Operator.Mulitiply:
                            return new Completion(l * r);
                        case Operator.Divide:
                            return new Completion(l / r);
                        case Operator.Great:
                            return new Completion(l > r);
                        case Operator.GreatOrEqual:
                            return new Completion(l >= r);
                        case Operator.Less:
                            return new Completion(l < r);
                        case Operator.LessOrEqual:
                            return new Completion(l <= r);
                        case Operator.Equal:
                            return new Completion(l == r);
                    }
                    return Completion.Exception("Unknow Exception", this);
                }
                catch(Exception e)
                {
                    return new Completion(e.Message, CompletionType.Exception);
                }
            }
            if(T.Equals(typeof(float)))
            {
                try
                {
                    var l = TypeConverters.GetValue<float>(left.ReturnValue);
                    var r = TypeConverters.GetValue<float>(right.ReturnValue);
                    switch (Operator)
                    {
                        case Operator.Add:
                            return new Completion(l + r);
                        case Operator.Minus:
                            return new Completion(l - r);
                        case Operator.Mulitiply:
                            return new Completion(l * r);
                        case Operator.Divide:
                            return new Completion(l / r);
                        case Operator.Great:
                            return new Completion(l > r);
                        case Operator.GreatOrEqual:
                            return new Completion(l >= r);
                        case Operator.Less:
                            return new Completion(l < r);
                        case Operator.LessOrEqual:
                            return new Completion(l <= r);
                        case Operator.Equal:
                            return new Completion(l == r);
                    }
                    return Completion.Exception("Unknow Exception", this);
                }
                catch (Exception e)
                {
                    return new Completion(e.Message, CompletionType.Exception);
                }
            }
            if(T.Equals(typeof(long)))
            {
                try
                {
                    var l = TypeConverters.GetValue<long>(left.ReturnValue);
                    var r = TypeConverters.GetValue<long>(right.ReturnValue);
                    switch (Operator)
                    {
                        case Operator.Add:
                            return new Completion(l + r);
                        case Operator.Minus:
                            return new Completion(l - r);
                        case Operator.Mulitiply:
                            return new Completion(l * r);
                        case Operator.Divide:
                            return new Completion(l / r);
                        case Operator.Great:
                            return new Completion(l > r);
                        case Operator.GreatOrEqual:
                            return new Completion(l >= r);
                        case Operator.Less:
                            return new Completion(l < r);
                        case Operator.LessOrEqual:
                            return new Completion(l <= r);
                        case Operator.Equal:
                            return new Completion(l == r);
                    }
                    return Completion.Exception("Unknow Exception", this);
                }
                catch (Exception e)
                {
                    return new Completion(e.Message, CompletionType.Exception);
                }
            }
            {
                try
                {
                    var l = TypeConverters.GetValue<double>(left.ReturnValue);
                    var r = TypeConverters.GetValue<double>(right.ReturnValue);
                    switch (Operator)
                    {
                        case Operator.Add:
                            return new Completion(l + r);
                        case Operator.Minus:
                            return new Completion(l - r);
                        case Operator.Mulitiply:
                            return new Completion(l * r);
                        case Operator.Divide:
                            return new Completion(l / r);
                        case Operator.Great:
                            return new Completion(l > r);
                        case Operator.GreatOrEqual:
                            return new Completion(l >= r);
                        case Operator.Less:
                            return new Completion(l < r);
                        case Operator.LessOrEqual:
                            return new Completion(l <= r);
                        case Operator.Equal:
                            return new Completion(l == r);
                    }
                    return Completion.Exception("Unknow Exception", this);
                }
                catch (Exception e)
                {
                    return Completion.Exception(e.Message, this);
                }
            }
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
