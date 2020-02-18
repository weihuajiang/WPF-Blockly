using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class ForStatement : Loop
    {
        public Expression Init { get; set; }
        public Expression Test { get; set; }
        public Expression Update { get; set; }
        public BlockStatement Body { get; set; } = new BlockStatement();

        public string ReturnType => "any";

        public Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "For("));
                desc.Add(new ExpressionDescriptor(this, "Init", "number"));
                desc.Add(new TextItemDescriptor(this, ";"));
                desc.Add(new ExpressionDescriptor(this, "Test", "boolean"));
                desc.Add(new TextItemDescriptor(this, ";"));
                desc.Add(new ExpressionDescriptor(this, "Update", "number"));
                desc.Add(new TextItemDescriptor(this, ")"));
                return desc;
            }
        }
        public BlockDescriptor BlockDescriptor
        {
            get
            {
                BlockDescriptor blockDescript = new BlockDescriptor();
                blockDescript.Add(new BlockStatementDescriptor(this, "Body"));
                return blockDescript;
            }
        }
        public Completion Execute(ExecutionEnvironment enviroment)
        {
            if (Init != null)
            {
                var c = Init.Execute(enviroment);
                if (c.Type != CompletionType.Value)
                    return c;
            }
            Completion cx = Completion.Void;
            while (true)
            {
                bool next = true;
                if (Test != null)
                {
                    var c = Test.Execute(enviroment);
                    if (c.Type != CompletionType.Value)
                        return c;
                }
                if (next)
                {
                    var current = new ExecutionEnvironment(enviroment);
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
                        var c = Update.Execute(enviroment);
                        if (c.Type != CompletionType.Value)
                            return c;
                    }
                }
            }
        }
        public string Type
        {
            get
            {
                return "ForStatement";
            }
        }
        public bool IsClosing
        {
            get { return false; }
        }

    }
}
