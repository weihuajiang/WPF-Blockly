using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class ConcateExpression// : Expression
    {
        public Expression Left { get; set; }
        public string Operator { get; set; }
        public Expression Right { get; set; }

        public string ReturnType
        {
            get { return "string"; }
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
                desc.Add(new TextItemDescriptor(this, "Concate ("));
                desc.Add(new ExpressionDescriptor(this, "Left", "string"));
                desc.Add(new TextItemDescriptor(this, " , )"));
                desc.Add(new ExpressionDescriptor(this, "Right", "string"));
                return desc;
            }
        }

        public string Type
        {
            get { return "ConcateExpression"; }
        }
        public bool IsClosing
        {
            get { return false; }
        }
    }
}
