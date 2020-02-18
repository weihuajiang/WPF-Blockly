using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class NotExpression : Expression
    {
        public Expression Argument { get; set; }
        public  string ReturnType
        {
            get { return "boolean"; }
        }

        public Completion Execute(ExecutionEnvironment enviroment)
        {
            if (Argument == null)
                return new Completion("value can not be null", CompletionType.Exception);
            var a = Argument.Execute(enviroment);
            if (a.Type != CompletionType.Value || !(a.ReturnValue is bool))
                return new Completion("value is not a boolean value", CompletionType.Exception);
            return new Completion(!(bool)a.ReturnValue);
        }

        public Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "not "));
                desc.Add(new ExpressionDescriptor(this, "Argument", "boolean"));
                return desc;
            }
        }
        bool result;
        public string Type
        {
            get { return "UnaryExpression"; }
        }
        public bool IsClosing
        {
            get { return false; }
        }
    }
}
