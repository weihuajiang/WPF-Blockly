using ScratchNet.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class StringLengthExpression : Expression
    {
        public Expression String { get; set; }
        public override string ReturnType => "number";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new ExpressionDescriptor(this, "String", "string") { NothingAllowed = true });
                desc.Add(new TextItemDescriptor(this, ".length()"));
                return desc;
            }
        }

        public override string Type => "StringLengthExpression";

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (String == null)
                return Completion.Exception(Language.NullException, this);
            var c = String.Execute(enviroment);
            if (!c.IsValue)
                return c;
            if (!(c.ReturnValue is string))
                return Completion.Exception(Language.NotStringException, String);
            return new Completion(((string)c.ReturnValue).Length);
        }
    }
}
