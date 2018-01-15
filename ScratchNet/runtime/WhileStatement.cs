using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{

    public class WhileStatement : Statement
    {
        public WhileStatement()
        {
            Body = new BlockStatement();
        }
        public Expression Test { get; set; }
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
                desc.Add(new TextItemDescriptor(this, "While("));
                desc.Add(new ExpressionDescriptor(this, "Test", "boolean"));
                desc.Add(new TextItemDescriptor(this, " )"));
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
    }
}
