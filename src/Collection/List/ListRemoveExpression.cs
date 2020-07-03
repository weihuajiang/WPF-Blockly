using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class ListRemoveExpression : Expression
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
                desc.Add(new TextItemDescriptor(this, ".remove("));
                desc.Add(new ExpressionDescriptor(this, "Value", "any"));
                desc.Add(new TextItemDescriptor(this, ")"));
                return desc;
            }
        }

        public override string Type => "ListRemoveExpression";

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (List == null || Value == null)
                return Completion.Exception("Null Exception", this);
            Completion c = List.Execute(enviroment);
            if (!c.IsValue)
                return c;
            Completion v = Value.Execute(enviroment);
            if (!v.IsValue)
                return v;
            if ((c.ReturnValue is List<object>))
            {
                List<object> stack = c.ReturnValue as List<object>;
                if (stack.Remove(v.ReturnValue))
                    return new Completion(v.ReturnValue);
                else
                    return Completion.Void;
            }
            else if (c.ReturnValue is Dictionary<object, object>)
            {
                Dictionary<object, object> d = c.ReturnValue as Dictionary<object, object>;
                if (d.ContainsKey(v.ReturnValue))
                {
                    var dv = d[v.ReturnValue];
                    d.Remove(v.ReturnValue);
                    return new Completion(dv);
                }
                else
                    return Completion.Void;
            }
            else
                return Completion.Exception("Only list or dictionary is accepted", List);
        }
    }
}
