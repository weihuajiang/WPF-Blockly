using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class PeekExpression : Expression
    {
        public Expression Stack { get; set; }
        public override string ReturnType => "any";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new ExpressionDescriptor(this, "Stack", "any") { NothingAllowed = true });
                desc.Add(new TextItemDescriptor(this, ".peek()"));
                return desc;
            }
        }

        public override string Type => "PeekStackExpression";

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Stack == null)
                return Completion.Exception("Null Exception", this);
            Completion c = Stack.Execute(enviroment);
            if (!c.IsValue)
                return c;
            if (c.ReturnValue is Stack<object>)
            {
                Stack<object> stack = c.ReturnValue as Stack<object>;
                return new Completion(stack.Peek());
            }
            else if(c.ReturnValue is Queue<object>)
            {
                Queue<object> stack = c.ReturnValue as Queue<object>;
                return new Completion(stack.Peek());
            }
            else
                return Completion.Exception("Only stack or queue value is accepted here", Stack);
        }
    }
}
