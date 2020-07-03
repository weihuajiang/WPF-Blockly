using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class DequeueExpression : Expression
    {
        public Expression Queue { get; set; }
        public override string ReturnType => "any";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new ExpressionDescriptor(this, "Queue", "any") { NothingAllowed = true });
                desc.Add(new TextItemDescriptor(this, ".dequeue()"));
                return desc;
            }
        }

        public override string Type => "DequeueExpression";

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Queue == null)
                return Completion.Exception("Null Exception", this);
            Completion c = Queue.Execute(enviroment);
            if (!c.IsValue)
                return c;
            if (!(c.ReturnValue is Queue<object>))
                return Completion.Exception("Only queue value is accepted here", Queue);
            Queue<object> stack = c.ReturnValue as Queue<object>;
            return new Completion(stack.Dequeue());
        }
    }
}
