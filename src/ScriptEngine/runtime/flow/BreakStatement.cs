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
        public override string ReturnType
        {
            get { return "void"; }
        }
        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            return new Completion(null, CompletionType.Break);
        }

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "break", true));
                desc.Add(new TextItemDescriptor(this, ";"));
                return desc;
            }
        }
        public override BlockDescriptor BlockDescriptor
        {
            get
            {
                return null;
            }
        }
        public override string Type
        {
            get
            {
                return "WhileStatement";
            }
        }
        public override bool IsClosing
        {
            get { return true; }
        }
        
    }
}
