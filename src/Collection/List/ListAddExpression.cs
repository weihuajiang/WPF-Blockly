using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class ListAddExpression : Expression
    {
        public Expression List { get; set; }
        public Expression Value { get; set; }
        public override string ReturnType => "any";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new ExpressionDescriptor(this, "List", "any") { NothingAllowed = true });
                desc.Add(new TextItemDescriptor(this, ".add("));
                desc.Add(new ExpressionDescriptor(this, "Value", "any"));
                desc.Add(new TextItemDescriptor(this, ")"));
                return desc;
            }
        }

        public override string Type => "ListAddExpression";

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (List == null || Value == null)
                return Completion.Exception("Null Exception", this);
            Completion c = List.Execute(enviroment);
            if (!c.IsValue)
                return c;
            if (!(c.ReturnValue is List<object>))
                return Completion.Exception("Only list value is accepted here", List);
            Completion v = Value.Execute(enviroment);
            if (!v.IsValue)
                return v;
            List<object> stack = c.ReturnValue as List<object>;
            stack.Add(v.ReturnValue);
            return new Completion(v.ReturnValue);
        }
    }
}
