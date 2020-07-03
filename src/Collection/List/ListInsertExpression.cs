using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class ListInsertExpression : Expression
    {
        public Expression List { get; set; }
        public Expression Value { get; set; } = new Literal("value");
        public Expression Index { get; set; } = new Literal("index");
        public override string ReturnType => "any";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new ExpressionDescriptor(this, "List", "any") { NothingAllowed = true });
                desc.Add(new TextItemDescriptor(this, ".insert("));
                desc.Add(new ExpressionDescriptor(this, "Index", "number") { IsOnlyNumberAllowed = true });
                desc.Add(new TextItemDescriptor(this, ", "));
                desc.Add(new ExpressionDescriptor(this, "Value", "any"));
                desc.Add(new TextItemDescriptor(this, ")"));
                return desc;
            }
        }

        public override string Type => "ListInsertExpression";

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (List == null || Value == null || Index==null)
                return Completion.Exception("Null Exception", this);
            Completion c = List.Execute(enviroment);
            if (!c.IsValue)
                return c;
            if (!(c.ReturnValue is List<object>))
                return Completion.Exception("Only list value is accepted here", List); 
            Completion i = Index.Execute(enviroment);
            if (!i.IsValue)
                return i;
            if (!(i.ReturnValue is int))
                return Completion.Exception("only integer value is accepted here", Index);
            Completion v = Value.Execute(enviroment);
            if (!v.IsValue)
                return v;
            List<object> stack = c.ReturnValue as List<object>;
            try
            {
                stack.Insert((int)i.ReturnValue, v.ReturnValue);
            }catch(Exception e)
            {
                return Completion.Exception(e.Message, Index);
            }
            return new Completion(v.ReturnValue);
        }
    }
}
