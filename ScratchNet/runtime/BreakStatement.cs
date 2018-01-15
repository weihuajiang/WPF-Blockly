using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class BreakStatement : Statement
    {
        public BreakStatement()
        {
        }
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
                desc.Add(new TextItemDescriptor(this, "Break"));
                return desc;
            }
        }
        public BlockDescriptor BlockDescriptor
        {
            get
            {
                return null;
            }
        }
        public string Type
        {
            get
            {
                return "WhileStatement";
            }
        }
    }
}
