using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class StringExpression : Expression
    {
        public string Raw { get; set; }
        public StringExpression()
        {

        }
        public StringExpression(string value)
        {
            Raw = value;
        }
        public override string ReturnType => "string";

        public override Descriptor Descriptor
        {
            get {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "\""));
                desc.Add(new StringInputDesciptor(this, "Raw"));
                desc.Add(new TextItemDescriptor(this, "\""));
                return desc;
            }
        }

        public override string Type => "StringExpression";

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            return new Completion(Literal.GetStringLateral(Raw));
        }
    }
}
