using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class IfStatement : Statement
    {
        public IfStatement()
        {
            Consequent = new BlockStatement();
            Alternate = new BlockStatement();
        }
        public Expression Test { get; set; }
        public BlockStatement Consequent
        {
            get;
            set;
        }
        public BlockStatement Alternate { get; set; }
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
                desc.Add(new TextItemDescriptor(this, "If "));
                desc.Add(new ExpressionDescriptor(this, "Test", "boolean"));
                desc.Add(new TextItemDescriptor(this, " then"));
                return desc;
            }
        }
        public BlockDescriptor BlockDescriptor
        {
            get
            {
                BlockDescriptor desc = new BlockDescriptor();
                desc.Add(new BlockStatementDescriptor(this, "Consequent"));
                desc.Add(new TextBlockStatementDescritor(this, "Alternate", "Else"));
                desc.Add(new BlockStatementDescriptor(this, "Alternate"));
                return desc;
            }
        }
        public string Type
        {
            get
            {
                return "IfStatement";
            }
        }
    }
}
