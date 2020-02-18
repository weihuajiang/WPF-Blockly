using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class ReturnStatement : Statement
    {
        public ReturnStatement()
        {
        }
        public Expression Expression { get; set; }
        public string ReturnType
        {
            get { return "void"; }
        }
        public Completion Execute(ExecutionEnvironment enviroment)
        {
            if(Expression==null)
                return new Completion(null, CompletionType.Return);
            var c = Expression.Execute(enviroment);
            if(c.Type== CompletionType.Value)
            {
                return new Completion(c.ReturnValue, CompletionType.Return);
            }
            return c;
        }

        public Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "Return "));
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
                return "ReturnStatement";
            }
        }
        public bool IsClosing
        {
            get { return true; }
        }
    }
}
