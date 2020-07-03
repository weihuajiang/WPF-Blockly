using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class CountExpression : Expression
    {
        public Expression Stack { get; set; }
        public override string ReturnType => "int";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new ExpressionDescriptor(this, "Stack", "any") { NothingAllowed = true });
                desc.Add(new TextItemDescriptor(this, ".count()"));
                return desc;
            }
        }

        public override string Type => "CountExpression";

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Stack == null)
                return Completion.Exception("Null Exception", this);
            Completion c = Stack.Execute(enviroment);
            if (!c.IsValue)
                return c;
            if (c.ReturnValue==null)
                return Completion.Exception("object value is null", Stack);
            Type t = c.ReturnValue.GetType();
            try
            {
                PropertyInfo p = t.GetProperty("Count");
                return new Completion(p.GetValue(c.ReturnValue));
            }
            catch { }
            return Completion.Exception("No count property found in object", Stack);
        }
    }
}
