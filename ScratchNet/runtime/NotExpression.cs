using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class NotExpression : Expression
    {
        public Expression Argument { get; set; }
        public  string ReturnType
        {
            get { return "boolean"; }
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
                desc.Add(new TextItemDescriptor(this, "not "));
                desc.Add(new ExpressionDescriptor(this, "Argument", "boolean"));
                return desc;
            }
        }

        public string Type
        {
            get { return "UnaryExpression"; }
        }
    }
}
