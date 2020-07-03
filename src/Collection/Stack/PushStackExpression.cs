using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class PushStackExpression : Expression
    {
        public Expression Stack { get; set; }
        public Expression Value { get; set; }
        public override string ReturnType => "any";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new ExpressionDescriptor(this, "Stack", "any") { NothingAllowed = true });
                desc.Add(new TextItemDescriptor(this, ".push("));
                desc.Add(new ExpressionDescriptor(this, "Value", "any"));
                desc.Add(new TextItemDescriptor(this, ")"));
                return desc;
            }
        }

        public override string Type => "PushStackExpression";

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Stack == null || Value==null)
                return Completion.Exception("Null Exception", this);
            Completion c = Stack.Execute(enviroment);
            if (!c.IsValue)
                return c;
            if (!(c.ReturnValue is Stack<object>))
                return Completion.Exception("Only stack value is accepted here", Stack);
            Completion v = Value.Execute(enviroment);
            if (!v.IsValue)
                return v;
            Stack<object> stack = c.ReturnValue as Stack<object>;
            stack.Push(v.ReturnValue);
            return new Completion(v.ReturnValue);
        }
    }
}
