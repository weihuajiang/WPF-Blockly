using ScratchNet.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class ParseLongExpression : Expression
    {
        public Expression String { get; set; }
        public override string ReturnType => "number";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "("));
                desc.Add(new TextItemDescriptor(this, "long", true));
                desc.Add(new TextItemDescriptor(this, ")"));
                desc.Add(new ExpressionDescriptor(this, "String", "string"));
                return desc;
            }
        }

        public override string Type => "ParseLongExpression";

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (String == null)
                return Completion.Exception(Language.NullException, this);
            var c = String.Execute(enviroment);
            if (!c.IsValue)
                return c;
            if (!(c.ReturnValue is string || TypeConverters.IsNumber(c.ReturnValue)))
                return Completion.Exception(Language.NotStringNumberException, String);
            if (c.ReturnValue is string)
            {
                try
                {
                    return new Completion(long.Parse(c.ReturnValue as string));
                }
                catch (Exception e)
                {
                    return Completion.Exception(e.Message, String);
                }
            }
            try
            {
                return new Completion(TypeConverters.GetValue<long>(c.ReturnValue));
            }
            catch (Exception e)
            {
                return Completion.Exception(string.Format(Language.NotConvertToLongException, c.ReturnValue), String);
            }
        }
    }
}
