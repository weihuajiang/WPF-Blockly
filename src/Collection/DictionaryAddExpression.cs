using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class DictionaryAddExpression : Expression
    {
        public Expression Dictionary { get; set; }
        public Expression Key { get; set; }
        public Expression Value { get; set; }
        public override string ReturnType => "any";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new ExpressionDescriptor(this, "Dictionary", "any") { NothingAllowed = true });
                desc.Add(new TextItemDescriptor(this, ".add("));
                desc.Add(new ExpressionDescriptor(this, "Key", "any"));
                desc.Add(new TextItemDescriptor(this, ","));
                desc.Add(new ExpressionDescriptor(this, "Value", "any"));
                desc.Add(new TextItemDescriptor(this, ")"));
                return desc;
            }
        }

        public override string Type => "DictionaryAddExpression";

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Dictionary == null || Key == null)
                return Completion.Exception("Null Exception", this);
            Completion c = Dictionary.Execute(enviroment);
            if (!c.IsValue)
                return c;
            if (!(c.ReturnValue is Dictionary<object, object>))
                return Completion.Exception("Value is not a dictionary", Dictionary);
            Completion k = Key.Execute(enviroment);
            if (!k.IsValue)
                return k;
            Dictionary<object, object> stack = c.ReturnValue as Dictionary<object, object>;
            var v = Value.Execute(enviroment);
            if (!v.IsValue)
                return v;
            stack.Add(k.ReturnValue, v.ReturnValue);
            return new Completion(v.ReturnValue);
        }
    }
}
