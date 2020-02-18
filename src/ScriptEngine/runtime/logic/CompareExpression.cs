using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class CompareExpression : Expression
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
                    return new Completion(l + r);
                }
                catch (Exception e)
                {
                    return new Completion(e.Message, CompletionType.Exception);
                }
            }
            if (T.Equals(typeof(float)))
            {
                try
                {
                    var l = TypeConverters.GetValue<float>(left.ReturnValue);
                    var r = TypeConverters.GetValue<float>(right.ReturnValue);
                    switch (Operator)
                    {
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
                    return new Completion(l + r);
                }
                catch (Exception e)
                {
                    return new Completion(e.Message, CompletionType.Exception);
                }
            }
            if (T.Equals(typeof(long)))
            {
                try
                {
                    var l = TypeConverters.GetValue<long>(left.ReturnValue);
                    var r = TypeConverters.GetValue<long>(right.ReturnValue);
                    switch (Operator)
                    {
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
                    return new Completion(l + r);
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
                    return new Completion(l + r);
                }
                catch (Exception e)
                {
                    return new Completion(e.Message, CompletionType.Exception);
                }
            }
        }

        public Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new ExpressionDescriptor(this, "Left", "number"));
                desc.Add(new TextItemDescriptor(this, " "));
                desc.Add(new SelectionItemDescriptor(this, "Operator",
                    new object[]{"==","!=",">",">=","<","<="},
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
    }
}
