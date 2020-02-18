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
        public string ReturnType
        {
            get { return "number|string|boolean"; }
        }

        public Completion Execute(ExecutionEnvironment enviroment)
        {
            if (Test == null)
                return Completion.Exception("Test can not be null", this);
            var t = Test.Execute(enviroment);
            if (t.ReturnValue is bool)
            {
                if ((bool)t.ReturnValue)
                {
                    if (Consequent == null)
                        return Completion.Exception("Consequent can not be null", this);
                    return Consequent.Execute(enviroment);
                }
                else
                {
                    if (Alternate == null)
                        return Completion.Exception("Alternate can not be null", this);
                    return Alternate.Execute(enviroment);
                }
            }
            else
                return new Completion("Test return not boolean value", CompletionType.Exception, this);
        }

        public Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new ExpressionDescriptor(this, "Test", "boolean"));
                desc.Add(new TextItemDescriptor(this, " ? "));
                desc.Add(new ExpressionDescriptor(this, "Consequent", "number|string|boolean"));
                desc.Add(new TextItemDescriptor(this, " : "));
                desc.Add(new ExpressionDescriptor(this, "Alternate", "number|string|boolean"));
                return desc;
            }
        }

        public string Type
        {
            get { return "ConditionalExpression"; }
        }
        public bool IsClosing
        {
            get { return false; }
        }
    }
}
