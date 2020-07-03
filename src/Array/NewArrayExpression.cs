using ScratchNet.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class NewArrayExpression : Expression
    {
        public Expression Length { get; set; }

        public override string ReturnType => "array";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "new", true));
                desc.Add(new TextItemDescriptor(this, " Array["));
                desc.Add(new ExpressionDescriptor(this, "Length", "number") { IsOnlyNumberAllowed = true });
                desc.Add(new TextItemDescriptor(this, "]"));
                return desc;
            }
        }

        public override string Type => "NewArrayExpression";

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Length == null)
                return Completion.Exception(Language.NullException, this);
            Completion c = Length.Execute(enviroment);
            if (!c.IsValue)
                return c;
            if (!(c.ReturnValue is int))
                return Completion.Exception(Language.NotNumberException, Length);
            return new Completion(new object[(int)c.ReturnValue]);
        }
    }
    public class NewArray2Expression : Expression
    {
        public Expression Length1 { get; set; }
        public Expression Length2 { get; set; }

        public override string ReturnType => "array";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "new", true));
                desc.Add(new TextItemDescriptor(this, " Array["));
                desc.Add(new ExpressionDescriptor(this, "Length1", "number") { IsOnlyNumberAllowed = true });
                desc.Add(new TextItemDescriptor(this, "]["));
                desc.Add(new ExpressionDescriptor(this, "Length2", "number") { IsOnlyNumberAllowed = true });
                desc.Add(new TextItemDescriptor(this, "]"));
                return desc;
            }
        }

        public override string Type => "NewArrayExpression";

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Length1 == null || Length2==null)
                return Completion.Exception(Language.NullException, this);
            Completion c = Length1.Execute(enviroment);
            if (!c.IsValue)
                return c;
            if (!(c.ReturnValue is int))
                return Completion.Exception(Language.NotNumberException, Length1);
            Completion l = Length2.Execute(enviroment);
            if (!l.IsValue)
                return l;
            if(!(l.ReturnValue is int))
                return Completion.Exception(Language.NotNumberException, Length2);
            return new Completion(new object[(int)c.ReturnValue, (int)l.ReturnValue]);
        }
    }
}
