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
        public string ValueType { get; set; } = "any";
        public string PropertyType { get; set; } = "any";
        public override string ReturnType
        {
            get
            {
                return ValueType;
            }
        }
        bool IsLogicalOperator(Operator op)
        {
            if (op == Operator.And || op == Operator.Or)
                return true;
            return false;
        }
        public bool IsCompareOperator(Operator op)
        {
            if (op == Operator.Equal || op == Operator.Less || op== Operator.LessOrEqual || op== Operator.Great || op== Operator.GreatOrEqual)
                return true;
            return false;
        }

        Completion ExecuteLogical(ExecutionEnvironment enviroment, object left, object right)
        {
            if (!(left is bool))
                return Completion.Exception(Properties.Language.NotBoolean, Left);
            if (!(right is bool))
                return Completion.Exception(Properties.Language.NotBoolean, Right);
            try
            {
                var l = TypeConverters.GetValue<bool>(left);
                var r = TypeConverters.GetValue<bool>(right);
                switch (Operator)
                {
                    case Operator.And:
                        return new Completion(l && r);
                    case Operator.Or:
                        return new Completion(l || r);
                }
                return Completion.Exception(Properties.Language.UnknowException, this);
            }
            catch (Exception e)
            {
                return Completion.Exception(e.Message, this);
            }
        }
        Completion ExecuteStringAdd(ExecutionEnvironment environment, object left, object right)
        {
            return new Completion(left + "" + right);
        }
        Completion ExecuteDateMinus(ExecutionEnvironment environment, object left, object right)
        {
            return new Completion((DateTime)left - (DateTime)right);
        }
        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Left == null)
                return Completion.Exception(Properties.Language.NullException, this);
            if (Right == null)
                return Completion.Exception(Properties.Language.NullException, this);
            var left = Left.Execute(enviroment);
            if (left.Type != CompletionType.Value)
                return left;
            var right = Right.Execute(enviroment);
            if (right.Type != CompletionType.Value)
                return right;
            if (IsLogicalOperator(Operator))
            {
                return ExecuteLogical(enviroment, left.ReturnValue, right.ReturnValue);
            }
            if(Operator== Operator.Add)
            {
                if ((left.ReturnValue is string) || right.ReturnValue is string)
                    return ExecuteStringAdd(enviroment, left.ReturnValue, right.ReturnValue);
            }
            if (Operator == Operator.Equal)
            {
                if (left.ReturnValue is string)
                    return new Completion(left.ReturnValue is string && (left.ReturnValue as string).Equals(right.ReturnValue));
                if(right.ReturnValue is string)
                    return new Completion(right.ReturnValue is string && (right.ReturnValue as string).Equals(left.ReturnValue));
                if(left.ReturnValue is bool)
                {
                    return new Completion(right.ReturnValue is bool && (bool)right.ReturnValue == (bool)left.ReturnValue);
                }
                if (right.ReturnValue is bool)
                {
                    return new Completion(left.ReturnValue is bool && (bool)left.ReturnValue == (bool)right.ReturnValue);
                }
                if (left.ReturnValue == null)
                {
                    if (right.ReturnValue == null)
                        return new Completion(true);
                    else
                        return new Completion(false);
                }
                if (right.ReturnValue == null)
                {
                    if (left.ReturnValue == null)
                        return new Completion(true);
                    return new Completion(false);
                }
            }
            if (Operator == Operator.NotEqual)
            {
                if (left.ReturnValue is string)
                    return new Completion(!(left.ReturnValue as string).Equals(right.ReturnValue));
                if (right.ReturnValue is string)
                    return new Completion(!(right.ReturnValue as string).Equals(left.ReturnValue)); if (left.ReturnValue is bool)
                {
                    return new Completion(!(right.ReturnValue is bool) || (bool)right.ReturnValue != (bool)left.ReturnValue);
                }
                if (right.ReturnValue is bool)
                {
                    return new Completion(!(left.ReturnValue is bool) || (bool)left.ReturnValue != (bool)right.ReturnValue);
                }
                if (left.ReturnValue == null)
                {
                    if (right.ReturnValue != null)
                        return new Completion(true);
                    else
                        return new Completion(false);
                }
                if (right.ReturnValue == null)
                {
                    if (left.ReturnValue != null)
                        return new Completion(true);
                    return new Completion(false);
                }
            }
            if (Operator == Operator.Add)
            {
                if ((left.ReturnValue is DateTime) && right.ReturnValue is DateTime)
                    return ExecuteStringAdd(enviroment, left.ReturnValue, right.ReturnValue);
            }
            Type T = TypeConverters.GetMaxTypes(left.ReturnValue, right.ReturnValue);

            if (T == null)
                return Completion.Exception(Properties.Language.NotNumber, this);
            if (Operator == Operator.Mod || Operator== Operator.BitAnd || Operator==Operator.BitOr ||
                Operator==Operator.BitLeftShift || Operator==Operator.BitRightShift || Operator== Operator.BitExclusiveOr)
                T = typeof(int);
            if (T.Equals(typeof(char)))
            {
                try
                {
                    var l = TypeConverters.GetValue<char>(left.ReturnValue);
                    var r = TypeConverters.GetValue<char>(right.ReturnValue);
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
                        case Operator.Mod:
                            return new Completion(l % r);
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
                        case Operator.NotEqual:
                            return new Completion(l != r);
                        case Operator.BitAnd:
                            return new Completion(l & r);
                        case Operator.BitOr:
                            return new Completion(l | r);
                        case Operator.BitLeftShift:
                            return new Completion(l << r);
                        case Operator.BitRightShift:
                            return new Completion(l >> r);
                        case Operator.BitExclusiveOr:
                            return new Completion(l ^ r);
                    }
                    return Completion.Exception(Properties.Language.UnknowException, this);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    return Completion.Exception(e.Message, this);
                }
            }
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
                        case Operator.Mod:
                            return new Completion(l % r);
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
                        case Operator.NotEqual:
                            return new Completion(l != r);
                        case Operator.BitAnd:
                            return new Completion(l & r);
                        case Operator.BitOr:
                            return new Completion(l | r);
                        case Operator.BitLeftShift:
                            return new Completion(l << r);
                        case Operator.BitRightShift:
                            return new Completion(l >> r);
                        case Operator.BitExclusiveOr:
                            return new Completion(l ^ r);
                    }
                    return Completion.Exception(Properties.Language.UnknowException, this);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    return Completion.Exception(e.Message, this);
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
                        case Operator.NotEqual:
                            return new Completion(l != r);
                    }
                    return Completion.Exception(Properties.Language.UnknowException, this);
                }
                catch (Exception e)
                {
                    return Completion.Exception(e.Message, this);
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
                        case Operator.NotEqual:
                            return new Completion(l != r);
                    }
                    return Completion.Exception(Properties.Language.UnknowException, this);
                }
                catch (Exception e)
                {
                    return Completion.Exception(e.Message, this);
                }
            }
            {
                try
                {
                    double l = TypeConverters.GetValue<double>(left.ReturnValue);
                    double r = TypeConverters.GetValue<double>(right.ReturnValue);
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
                        case Operator.NotEqual:
                            return new Completion(l != r);
                    }
                    return Completion.Exception(Properties.Language.UnknowException, this);
                }
                catch (Exception e)
                {
                    return Completion.Exception(e.Message, this);
                }
            }
        }

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new ExpressionDescriptor(this, "Left", PropertyType) );
                desc.Add(new TextItemDescriptor(this, " "));
                desc.Add(new SelectionItemDescriptor(this, "Operator", new object[] {"==","!=",">",">=","<","<=", "&&", "||", "+", "-", "*", "/","%","&","|","^" ,">>","<<"},
                    new object[] { 
                        Operator.Equal, Operator.NotEqual, Operator.Great, Operator.GreatOrEqual, Operator.Less, Operator.LessOrEqual,
                        Operator.And, Operator.Or,
                        Operator.Add, Operator.Minus, Operator.Mulitiply, Operator.Divide, Operator.Mod, Operator.BitAnd, 
                        Operator.BitOr, Operator.BitExclusiveOr, Operator.BitRightShift, Operator.BitLeftShift}));
                desc.Add(new TextItemDescriptor(this, " "));
                desc.Add(new ExpressionDescriptor(this, "Right", PropertyType));
                return desc;
            }
        }

        public override string Type
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
        Mod,
        Equal,
        Great,
        Less,
        NotEqual,
        GreatOrEqual,
        LessOrEqual,
        And,
        Or,
        BitAnd,
        BitOr,
        BitRightShift,
        BitLeftShift,
        BitExclusiveOr
    }
}
