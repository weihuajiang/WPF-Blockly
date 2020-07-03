using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class PopStackExpression : Expression
    {
        public Expression Stack { get; set; }
        public override string ReturnType => "any";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new ExpressionDescriptor(this, "Stack", "any") { NothingAllowed = true }) ;
                desc.Add(new TextItemDescriptor(this, ".pop()"));
                return desc;
            }
        }

        public override string Type => "PopStackExpression";

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Stack == null)
                return Completion.Exception("Null Exception", this);
            Completion c = Stack.Execute(enviroment);
            if (!c.IsValue)
                return c;
            if (!(c.ReturnValue is Stack<object>))
                return Completion.Exception("Only stack value is accepted here", Stack);
            Stack<object> stack = c.ReturnValue as Stack<object>;
            return new Completion(stack.Pop());
        }
    }
}
