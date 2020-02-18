using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class ForInStatement// : Loop
    {
        public ForInStatement()
        {
            Body = new BlockStatement();
        }
        public Expression Left { get; set; }
        public Expression Right { get; set; }
        public BlockStatement Body
        {
            get;
            set;
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
                desc.Add(new TextItemDescriptor(this, "For "));
                desc.Add(new ExpressionDescriptor(this, "Left", "number|boolean|string"));
                desc.Add(new TextItemDescriptor(this, " in "));
                desc.Add(new ExpressionDescriptor(this, "Right", "number|boolean|string"));
                return desc;
            }
        }
        public BlockDescriptor BlockDescriptor
        {
            get
            {
                BlockDescriptor desc = new BlockDescriptor();
                desc.Add(new BlockStatementDescriptor(this, "Body"));
                return desc;
            }
        }
        public string Type
        {
            get
            {
                return "WhileStatement";
            }
        }
        public bool IsClosing
        {
            get { return false; }
        }
    }
}
