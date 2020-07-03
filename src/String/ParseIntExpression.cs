using ScratchNet.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class ParseIntExpression : Expression
    {
        public Expression String { get; set; }
        public override string ReturnType => "number";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "("));
                desc.Add(new TextItemDescriptor(this, "int", true));
                desc.Add(new TextItemDescriptor(this, ")"));
                desc.Add(new ExpressionDescriptor(this, "String", "string"));
                return desc;
            }
        }

        public override string Type => "ParseIntExpression";

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (String == null)
                return Completion.Exception(Language.NullException, this);
            var c = String.Execute(enviroment);
            if (!c.IsValue)
                return c;
            if (!(c.ReturnValue is string || TypeConverters.IsNumber(c.ReturnValue)))
                return Completion.Exception(Language.NotStringNumberException, String);
            if(c.ReturnValue is string)
            {
                try
                {
                    return new Completion(int.Parse(c.ReturnValue as string));
                }catch(Exception e)
                {
                    return Completion.Exception(e.Message, String);
                }
            }
            try
            {
                return new Completion(TypeConverters.GetValue<int>(c.ReturnValue));
            }
            catch(Exception e)
            {
                return Completion.Exception(string.Format(Language.NotConvertToIntegerException, c.ReturnValue), String);
            }
        }
    }
}
