using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class ExpressionStatement : Statement
    {
        public Expression Expression { get; set; }
        public string ReturnType
        {
            get { return "boolean|number|string"; }
        }
        public Completion Execute(ExecutionEnvironment enviroment)
        {
            if (Expression == null)
                return new Completion(null);
            return Expression.Execute(enviroment);
            
        }

        public Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new ExpressionDescriptor(this, "Expression", "number|string|boolean"));
                return desc;
            }
        }
        public string Type
        {
            get
            {
                return "ExpressionStatement";
            }
        }


        public BlockDescriptor BlockDescriptor
        {
            get { return null; }
        }


        public bool IsClosing
        {
            get { return false; }
        }
    }
}
