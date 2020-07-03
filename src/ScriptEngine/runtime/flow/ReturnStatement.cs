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
        public override string ReturnType
        {
            get { return "void"; }
        }
        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
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

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "return ", true));
                if (Expression != null)
                {
                    desc.Add(new TextItemDescriptor(this, "("));
                    desc.Add(new ExpressionDescriptor(this, "Expression", "any") { IsOnlyNumberAllowed = false });
                    desc.Add(new TextItemDescriptor(this, ");"));
                }
                else
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
                return "ReturnStatement";
            }
        }
        public override bool IsClosing
        {
            get { return true; }
        }
    }
}
