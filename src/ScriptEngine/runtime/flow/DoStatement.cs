using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class DoStatement : Statement
    {
        public BlockStatement Body { get; set; } = new BlockStatement();
        public Expression Test { get; set; }
        public override string ReturnType => "any";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "do", true));
                desc.Add(new TextItemDescriptor(this, "            "));
                return desc;
            }

        }
        public override string Type => "DoStaement";

        public override BlockDescriptor BlockDescriptor
        {
            get
            {
                BlockDescriptor desc = new BlockDescriptor();
               // Descriptor doDesc = new Descriptor();
               // doDesc.Add(new TextItemDescriptor(this, "do", true));
               // desc.Add(new ExpressionStatementDescription(this, "do", doDesc));


                desc.Add(new BlockStatementDescriptor(this, "Body"));

                Descriptor d = new Descriptor();
                d.Add(new TextItemDescriptor(this, "while", true));
                d.Add(new TextItemDescriptor(this, "("));
                d.Add(new ExpressionDescriptor(this, "Test", "boolean") { IsOnlyNumberAllowed = false });
                d.Add(new TextItemDescriptor(this, ")  "));
                desc.Add(new ExpressionStatementDescription(this, "while", d));
                return desc;
            }
        }

        public override bool IsClosing => false;

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Test == null)
                return Completion.Exception(Properties.Language.TestNullException, this);
            while(true)
            {
                ExecutionEnvironment current = new ExecutionEnvironment(enviroment);
                if (Body != null)
                {
                    ExecutionEnvironment loopEnv = new ExecutionEnvironment(enviroment);
                    var c = Body.Execute(loopEnv);
                    if (c.Type == CompletionType.Exception || c.Type == CompletionType.Return)
                        return c;
                    if (c.Type == CompletionType.Continue)
                        continue;
                    if (c.Type == CompletionType.Break)
                        return Completion.Void;
                    var t = Test.Execute(enviroment);
                    if (!t.IsValue)
                        return t;
                    if (t.ReturnValue is bool)
                    {
                        if ((bool)t.ReturnValue)
                            continue;
                        else
                            return Completion.Void;
                    }
                    else
                        return Completion.Exception(Properties.Language.NotBoolean, Test);
                }
            }
        }
    }
}
