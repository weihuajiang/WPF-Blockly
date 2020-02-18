using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{

    public class WaitStatement : Statement
    {
        public Expression Duration { get; set; }
        public string ReturnType
        {
            get { return "void"; }
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
                desc.Add(new TextItemDescriptor(this, "Wait for "));
                desc.Add(new ExpressionDescriptor(this, "Duration", "number"));
                desc.Add(new TextItemDescriptor(this, " seconds"));
                return desc;
            }
        }
        public string Type
        {
            get
            {
                return "WaitExpression";
            }
        }


        public BlockDescriptor BlockDescriptor
        {
            get { return null; }
        }
    }
}
