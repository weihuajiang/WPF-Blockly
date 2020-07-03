using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class EnqueueExpression : Expression
    {
        public Expression Queue { get; set; }
        public Expression Value { get; set; }
        public override string ReturnType => "any";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new ExpressionDescriptor(this, "Queue", "any") { NothingAllowed = true });
                desc.Add(new TextItemDescriptor(this, ".enqueue("));
                desc.Add(new ExpressionDescriptor(this, "Value", "any"));
                desc.Add(new TextItemDescriptor(this, ")"));
                return desc;
            }
        }

        public override string Type => "EnqueueExpression";

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Queue == null || Value == null)
                return Completion.Exception("Null Exception", this);
            Completion c = Queue.Execute(enviroment);
            if (!c.IsValue)
                return c;
            if (!(c.ReturnValue is Queue<object>))
                return Completion.Exception("Only queue value is accepted here", Queue);
            Completion v = Value.Execute(enviroment);
            if (!v.IsValue)
                return v;
            Queue<object> stack = c.ReturnValue as Queue<object>;
            stack.Enqueue(v.ReturnValue);
            return new Completion(v.ReturnValue);
        }
    }
}
