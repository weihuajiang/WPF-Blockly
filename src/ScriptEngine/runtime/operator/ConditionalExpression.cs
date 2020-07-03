using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class ConditionalExpression: Expression
    {
        public Expression Test { get; set; }
        public Expression Consequent { get; set; }
        public Expression Alternate { get; set; }
        public override string ReturnType
        {
            get { return "number|string|boolean"; }
        }

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Test == null)
                return Completion.Exception(Properties.Language.TestNullException, this);
            var t = Test.Execute(enviroment);
            if (t.ReturnValue is bool)
            {
                if ((bool)t.ReturnValue)
                {
                    if (Consequent == null)
                        return Completion.Exception(Properties.Language.NullException, this);
                    return Consequent.Execute(enviroment);
                }
                else
                {
                    if (Alternate == null)
                        return Completion.Exception(Properties.Language.NullException, this);
                    return Alternate.Execute(enviroment);
                }
            }
            else
                return Completion.Exception(Properties.Language.NotBoolean, Test);
        }

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new ExpressionDescriptor(this, "Test", "boolean") { IsOnlyNumberAllowed = false });
                desc.Add(new TextItemDescriptor(this, " ? "));
                desc.Add(new ExpressionDescriptor(this, "Consequent", "number|string|boolean"));
                desc.Add(new TextItemDescriptor(this, " : "));
                desc.Add(new ExpressionDescriptor(this, "Alternate", "number|string|boolean"));
                return desc;
            }
        }

        public override string Type
        {
            get { return "ConditionalExpression"; }
        }
        public bool IsClosing
        {
            get { return false; }
        }
    }
}
