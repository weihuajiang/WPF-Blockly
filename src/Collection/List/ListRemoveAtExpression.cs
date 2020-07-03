using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class ListRemoveAtExpression : Expression
    {
        public Expression List { get; set; }
        public Expression Index { get; set; }
        public override string ReturnType => "void";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new ExpressionDescriptor(this, "List", "any") { NothingAllowed = true });
                desc.Add(new TextItemDescriptor(this, ".removeAt("));
                desc.Add(new ExpressionDescriptor(this, "Index", "number") { IsOnlyNumberAllowed = true });
                desc.Add(new TextItemDescriptor(this, ")"));
                return desc;
            }
        }

        public override string Type => "ListInsertExpression";

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (List == null || Index == null)
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
            int id = (int)i.ReturnValue;

            List<object> stack = c.ReturnValue as List<object>;
            if (id < 0 || id >= stack.Count)
            {
                return Completion.Exception("index was out of list index range", Index);
            }
            try
            {
                object ov = stack[id];
                stack.RemoveAt(id);
                return new Completion(ov);
            }
            catch (Exception e)
            {
                return Completion.Exception(e.Message, Index);
            }
        }
    }
}
