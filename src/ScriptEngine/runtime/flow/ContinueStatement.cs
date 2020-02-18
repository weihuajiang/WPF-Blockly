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
        public string ReturnType
        {
            get { return "void"; }
        }
        public Completion Execute(ExecutionEnvironment enviroment)
        {
            return new Completion(null, CompletionType.Continue);
        }

        public Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "Continue"));
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
                return "ContinueStatement";
            }
        }
        public bool IsClosing
        {
            get { return true; }
        }
    }
}
