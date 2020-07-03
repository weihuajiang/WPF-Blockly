using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class ExpressionStatement : Statement
    {
        public Expression Expression { get; set; }
        public override string ReturnType
        {
            get { return "boolean|number|string"; }
        }
        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Expression == null)
                return Completion.Void;
            return Expression.Execute(enviroment);
            
        }

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new ExpressionDescriptor(this, "Expression", "number|string|boolean") { AcceptVariableDeclaration = true, NothingAllowed = true });
                desc.Add(new TextItemDescriptor(this, ";"));
                return desc;
            }
        }
        public override string Type
        {
            get
            {
                return "ExpressionStatement";
            }
        }


        public override BlockDescriptor BlockDescriptor
        {
            get { return null; }
        }


        public override bool IsClosing
        {
            get { return false; }
        }
    }
}
