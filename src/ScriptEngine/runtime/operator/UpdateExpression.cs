using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class UpdateExpression : Expression
    {
        public Expression Expression { get; set; }
        public UpdateOperator Operator { get; set; } = UpdateOperator.Add;
        public bool IsPrefix { get; set; } = false;
        public override string ReturnType => "number";

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Expression == null)
                return Completion.Exception(Properties.Language.NullException, this);
            var c = Expression.Execute(enviroment);
            if (c.Type != CompletionType.Value)
                return c;
            if (!TypeConverters.IsNumber(c.ReturnValue))
                return Completion.Exception(Properties.Language.NotNumber, Expression);
            Type t = c.ReturnValue.GetType();
            if(t.Equals(typeof(int)))
            {
                int v = TypeConverters.GetValue<int>(c.ReturnValue);
                int old = v;
                if (Operator == UpdateOperator.Add)
                    v += 1;
                else
                    v = v - 1;
                if(Expression is Identifier)
                {
                    enviroment.SetValue(((Identifier)Expression).Variable, v);
                }
                return new Completion(IsPrefix ? v : old);
            }
            else if(t.Equals(typeof(float)))
            {
                float v = TypeConverters.GetValue<float>(c.ReturnValue);
                float old = v;
                if (Operator == UpdateOperator.Add)
                    v += 1;
                else
                    v = v - 1;
                if (Expression is Identifier)
                {
                    enviroment.SetValue(((Identifier)Expression).Variable, v);
                }
                return new Completion(IsPrefix ? v : old);
            }
            else 
            {
                double v = TypeConverters.GetValue<double>(c.ReturnValue);
                double old = v;
                if (Operator == UpdateOperator.Add)
                    v += 1;
                else
                    v = v - 1;
                if (Expression is Identifier)
                {
                    enviroment.SetValue(((Identifier)Expression).Variable, v);
                }
                return new Completion(IsPrefix ? v : old);
            }
        }
        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                if (IsPrefix)
                {
                    desc.Add(new SelectionItemDescriptor(this, "Operator", new object[] { "++", "--", },
                        new object[] { UpdateOperator.Add, UpdateOperator.Minus }));
                    desc.Add(new ExpressionDescriptor(this, "Expression", "number") { NothingAllowed = true });
                }
                else
                {
                    desc.Add(new ExpressionDescriptor(this, "Expression", "number") { NothingAllowed = true });
                    desc.Add(new SelectionItemDescriptor(this, "Operator", new object[] { "++", "--", },
                        new object[] { UpdateOperator.Add, UpdateOperator.Minus }));
                }
                return desc;
            }
        }

        public override string Type
        {
            get { return "UpdateExpression"; }
        }
        public bool IsClosing
        {
            get { return false; }
        }
    }
    public enum UpdateOperator
    {
        Add,
        Minus
    }
}
