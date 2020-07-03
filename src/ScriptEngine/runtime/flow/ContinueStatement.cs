using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class ContinueStatement : Statement
    {
        public ContinueStatement()
        {
        }
        public override string ReturnType
        {
            get { return "void"; }
        }
        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            return new Completion(null, CompletionType.Continue);
        }

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "continue", true));
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
                return "ContinueStatement";
            }
        }
        public override bool IsClosing
        {
            get { return true; }
        }
    }
}
