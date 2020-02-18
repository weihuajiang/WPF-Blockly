using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class LogicExpression :Expression
    {
        public Expression Left { get; set; }
        public Expression Right { get; set; }
        public string Operator { get; set; }
        public string ReturnType
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
                desc.Add(new ExpressionDescriptor(this, "Left", "boolean"));
                desc.Add(new TextItemDescriptor(this, Operator));
                desc.Add(new ExpressionDescriptor(this, "Right", "boolean"));
                return desc;
            }
        }

        public string Type
        {
            get { return "AddExpression"; }
        }
    }
}
