using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class ConditionalExpression: Expression
    {
        public Expression Test { get; set; }
        public Expression Consequent { get; set; }
        public Expression Alternate { get; set; }
        public string ReturnType
        {
            get { return "number|string|boolean"; }
        }

        public Completion Execute(ExecutionEnvironment enviroment)
        {
            return Completion.Void;
        }

        public Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new ExpressionDescriptor(this, "Test", "boolean"));
                desc.Add(new TextItemDescriptor(this, "?"));
                desc.Add(new ExpressionDescriptor(this, "Consequent", "number|string|boolean"));
                desc.Add(new TextItemDescriptor(this, ":"));
                desc.Add(new ExpressionDescriptor(this, "Alternate", "number|string|boolean"));
                return desc;
            }
        }

        public string Type
        {
            get { return "ConditionalExpression"; }
        }
    }
}
