using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class CallFuncationStatement : Statement
    {
        public string Function
        {
            get;
            set;
        }
        public CallFuncationStatement()
        {
            Args = new List<Expression>();
            ArgTyps = new List<string>();
        }
        public List<Expression> Args { get;set; }
        public List<string> ArgTyps { get; set; }
        public string ReturnType
        {
            get { return "number|boolean|string"; }
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
                desc.Add(new TextItemDescriptor(this, Function+"("));
                if (Args != null && Args.Count > 0)
                {
                    int i=0;
                    foreach (Expression p in Args)
                    {
                        if (i != 0)
                            desc.Add(new TextItemDescriptor(this, ", "));
                        string t = "";
                        if (ArgTyps != null && ArgTyps.Count >= i + 1)
                            t = ArgTyps[i];
                        desc.Add(new ArgumentDescriptor(this, i, "Args", t));
                        i++;
                    }
                }
                desc.Add(new TextItemDescriptor(this, ")"));
                return desc;
            }
        }

        public string Type
        {
            get { return ""; }
        }

        public BlockDescriptor BlockDescriptor
        {
            get { return null; }
        }
    }
}
