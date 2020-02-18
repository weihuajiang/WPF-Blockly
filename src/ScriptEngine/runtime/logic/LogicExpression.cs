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
            if (Left == null || Right == null)
                return new Completion("Null Exception", CompletionType.Exception);
            var left = Left.Execute(enviroment);
            if (left.Type != CompletionType.Value)
                return left;
            if (!(left.ReturnValue is bool))
                return Completion.Exception("left value is not a boolean", Left);
            var right = Right.Execute(enviroment);

            if (right.Type != CompletionType.Value)
                return right;
            if (!(right.ReturnValue is bool))
                return Completion.Exception("right value is not a boolean", Right);
            try
            {
                var l = TypeConverters.GetValue<bool>(left.ReturnValue);
                var r = TypeConverters.GetValue<bool>(right.ReturnValue);
                switch (Operator)
                {
                    case Operator.And:
                        return new Completion(l && r);
                    case Operator.Or:
                        return new Completion(l || r);
                }
                return Completion.Exception("Unknow Exception", this);
            }
            catch (Exception e)
            {
                return new Completion(e.Message, CompletionType.Exception);
            }
        }

        public Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new ExpressionDescriptor(this, "Left", "boolean"));
                desc.Add(new SelectionItemDescriptor(this, "Operator", new object[] { "&&", "||"},
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
    }
}
