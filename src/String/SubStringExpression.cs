using ScratchNet.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class SubStringExpression : Expression
    {
        public Expression String { get; set; }
        public Expression StartIndex { get; set; }
        public Expression Length { get; set; }
        public override string ReturnType => "number";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new ExpressionDescriptor(this, "String", "string") { NothingAllowed = true });
                desc.Add(new TextItemDescriptor(this, ".subString("));
                desc.Add(new ExpressionDescriptor(this, "StartIndex", "string") { IsOnlyNumberAllowed = true });
                desc.Add(new TextItemDescriptor(this, ","));
                desc.Add(new ExpressionDescriptor(this, "Length", "string") { IsOnlyNumberAllowed = true });
                desc.Add(new TextItemDescriptor(this, ")"));
                return desc;
            }
        }

        public override string Type => "";

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (String == null)
                return Completion.Exception(Language.NullException, this);
            var c = String.Execute(enviroment);
            if (!c.IsValue)
                return c;
            if (!(c.ReturnValue is string))
                return Completion.Exception(Language.NotStringException, String);

            if (StartIndex == null)
                return Completion.Exception(Language.NullException, this);
            var f = StartIndex.Execute(enviroment);
            if (!f.IsValue)
                return f;
            if (!(f.ReturnValue is int))
                return Completion.Exception(Language.NotStringNumberException, StartIndex);

            if (Length == null)
                return new Completion((c.ReturnValue as string).Substring((int)f.ReturnValue));
            var g = Length.Execute(enviroment);
            if (!g.IsValue)
                return g;
            if (!(g.ReturnValue is int))
                return Completion.Exception(Language.NotIntegerException, Length);

            return new Completion((c.ReturnValue as string).Substring((int)f.ReturnValue, (int)g.ReturnValue ));
        }
    }
}
