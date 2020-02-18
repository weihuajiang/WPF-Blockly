using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class BinaryExpression : Expression
    {
        public Expression Left { get; set; }
        public string Operator { get; set; }
        public Expression Right { get; set; }

        public string ReturnType
        {
            get { return "number"; }
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
                desc.Add(new ExpressionDescriptor(this, "Left", "number"));
                desc.Add(new TextItemDescriptor(this, ""+Operator+""));
                desc.Add(new ExpressionDescriptor(this, "Right", "number"));
                return desc;
            }
        }

        public string Type
        {
            get { return "BinaryExpression"; }
        }
        string _id = Guid.NewGuid().ToString();
        public override string ToString()
        {
            return Type + _id;
        }
    }
}
