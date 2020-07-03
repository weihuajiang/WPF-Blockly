using ScratchNet.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class IndexOfStringExpression : Expression
    {
        public Expression String { get; set; }
        public Expression SubString { get; set; } 
        public override string ReturnType => "number";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new ExpressionDescriptor(this, "String", "string") { NothingAllowed = true });
                desc.Add(new TextItemDescriptor(this, ".indexOf("));
                desc.Add(new ExpressionDescriptor(this, "SubString", "string") );
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

            if (SubString == null)
                return Completion.Exception(Language.NullException, this);
            var f = SubString.Execute(enviroment);
            if (!f.IsValue)
                return f;
            if (!(f.ReturnValue is string))
                return Completion.Exception(Language.NotStringException, SubString);
            return new Completion((c.ReturnValue as string).IndexOf(f.ReturnValue as string));
        }
    }
    public class LastIndexOfStringExpression : Expression
    {
        public Expression String { get; set; }
        public Expression SubString { get; set; }
        public override string ReturnType => "number";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new ExpressionDescriptor(this, "String", "string") { NothingAllowed = true });
                desc.Add(new TextItemDescriptor(this, ".lastIndexOf("));
                desc.Add(new ExpressionDescriptor(this, "SubString", "string"));
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

            if (SubString == null)
                return Completion.Exception(Language.NotStringException, this);
            var f = SubString.Execute(enviroment);
            if (!f.IsValue)
                return f;
            if (!(f.ReturnValue is string))
                return Completion.Exception(Language.NotStringException, SubString);
            return new Completion((c.ReturnValue as string).LastIndexOf(f.ReturnValue as string));
        }
    }
}
