using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class NewLinkedListExpression : Expression
    {
        public override string ReturnType => "any";
        public Expression Value { get; set; }
        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "new", true));
                desc.Add(new TextItemDescriptor(this, " LinkedList("));
                desc.Add(new ExpressionDescriptor(this, "Value", "any"));
                desc.Add(new TextItemDescriptor(this, " )"));
                return desc;
            }
        }

        public override string Type => "NewStackExpression";

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            object value = null;
            if (Value != null)
            {
                var c = Value.Execute(enviroment);
                if (!c.IsValue)
                    return c;
                value = c.ReturnValue;
            }
            return new Completion(new LinkedListEx(value));
        }
    }
}
