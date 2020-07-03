using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class ForStatement : Statement
    {
        public Expression Init { get; set; }
        public Expression Test { get; set; }
        public Expression Update { get; set; }
        public BlockStatement Body { get; set; } = new BlockStatement();

        public override string ReturnType => "any";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "for", true));
                desc.Add(new TextItemDescriptor(this, "("));
                desc.Add(new ExpressionDescriptor(this, "Init", "number") { AcceptVariableDeclaration = true }) ;
                desc.Add(new TextItemDescriptor(this, ";"));
                desc.Add(new ExpressionDescriptor(this, "Test", "boolean") { IsOnlyNumberAllowed = false });
                desc.Add(new TextItemDescriptor(this, ";"));
                desc.Add(new ExpressionDescriptor(this, "Update", "number"));
                desc.Add(new TextItemDescriptor(this, ")"));
                return desc;
            }
        }
        public override BlockDescriptor BlockDescriptor
        {
            get
            {
                BlockDescriptor blockDescript = new BlockDescriptor();
                blockDescript.Add(new BlockStatementDescriptor(this, "Body"));
                return blockDescript;
            }
        }
        protected override Completion ExecuteImpl(ExecutionEnvironment environment)
        {
            ExecutionEnvironment loop = new ExecutionEnvironment(environment);
            if (Init != null)
            {
                var c = Init.Execute(loop);
                if (c.Type != CompletionType.Value)
                    return c;
            }
            Completion cx = Completion.Void;
            while (true)
            {
                bool next = true;
                if (Test != null)
                {
                    var c = Test.Execute(loop);
                    if (c.Type != CompletionType.Value)
                        return c;
                    if (!(c.ReturnValue is bool))
                        return Completion.Exception(Properties.Language.NotBoolean, Test);
                    if (!(bool)c.ReturnValue)
                        return Completion.Void;
                }
                if (next)
                {
                    var current = new ExecutionEnvironment(loop);
                    cx = Body.Execute(current);
                    if (cx.Type == CompletionType.Exception)
                        return cx;
                    if (cx.Type == CompletionType.Return)
                        return cx;
                    if(cx.Type== CompletionType.Break)
                    {
                        return Completion.Void;
                    }
                    if (Update != null)
                    {
                        var c = Update.Execute(loop);
                        if (c.Type != CompletionType.Value)
                            return c;
                    }
                }
            }
        }
        public override string Type
        {
            get
            {
                return "ForStatement";
            }
        }
        public override bool IsClosing
        {
            get { return false; }
        }

    }
}
