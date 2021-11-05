using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class NotExpression : Expression
    {
        public Expression Argument { get; set; }
        public override string ReturnType
        {
            get { return "boolean"; }
        }

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "! ", true));
                desc.Add(new ExpressionDescriptor(this, "Argument", "boolean") { IsOnlyNumberAllowed = false });
                return desc;
            }
        }
        public override string Type
        {
            get { return "UnaryExpression"; }
        }

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Argument == null)
                return Completion.Exception(Properties.Language.NullException, this);
            var a = Argument.Execute(enviroment);
            if (a.Type != CompletionType.Value || !(a.ReturnValue is bool))
                return new Completion(Properties.Language.NotBoolean, CompletionType.Exception);
            return new Completion(!(bool)a.ReturnValue);
        }
    }
}
