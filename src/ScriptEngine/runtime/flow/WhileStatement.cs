using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{

    public class WhileStatement : Statement
    {
        public WhileStatement()
        {
            Body = new BlockStatement();
        }
        public Expression Test { get; set; }
        public BlockStatement Body
        {
            get;
            set;
        }
        public override string ReturnType
        {
            get { return "void"; }
        }
        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Test == null)
                return Completion.Exception(Properties.Language.TestNullException, this);
            var c = Test.Execute(enviroment);
            if (c.Type == CompletionType.Value)
            {
                if (c.ReturnValue is bool)
                {
                    Completion cx = Completion.Void;
                    while ((bool)c.ReturnValue)
                    {
                        ExecutionEnvironment current = new ExecutionEnvironment(enviroment);
                        cx = Body.Execute(current);
                        if (cx.Type == CompletionType.Exception)
                        {
                            return cx;
                        }
                        if (cx.Type == CompletionType.Break)
                        {
                            return Completion.Void;
                        }
                        if (cx.Type == CompletionType.Return)
                            return cx;
                        c = Test.Execute(enviroment);
                        if (!(c.ReturnValue is bool))
                            return Completion.Exception(Properties.Language.NotBoolean, Test);
                    }
                    return cx;
                }
                else
                    return Completion.Exception(Properties.Language.NotBoolean, Test);
            }
            return c;
        }

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "while", true));
                desc.Add(new TextItemDescriptor(this, "("));
                desc.Add(new ExpressionDescriptor(this, "Test", "boolean") { IsOnlyNumberAllowed = false });
                desc.Add(new TextItemDescriptor(this, " )"));
                return desc;
            }
        }
        public override BlockDescriptor BlockDescriptor
        {
            get
            {
                BlockDescriptor desc = new BlockDescriptor();
                desc.Add(new BlockStatementDescriptor(this, "Body"));
                return desc;
            }
        }
        public override string Type
        {
            get
            {
                return "WhileStatement";
            }
        }
        public override bool IsClosing
        {
            get { return false; }
        }
        
    }
}
