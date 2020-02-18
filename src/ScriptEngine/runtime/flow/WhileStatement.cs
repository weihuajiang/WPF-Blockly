using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{

    public class WhileStatement : Loop
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
        public string ReturnType
        {
            get { return "void"; }
        }
        public Completion Execute(ExecutionEnvironment enviroment)
        {
            if (Test == null || Body == null || Body.Body.Count == 0)
                return Completion.Void;
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
                            return Completion.Exception("value is not boolean", Test);
                    }
                    return cx;
                }
                else
                    return Completion.Exception("test variable is not boolean", Test);
            }
            return c;
        }

        public Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "While("));
                desc.Add(new ExpressionDescriptor(this, "Test", "boolean"));
                desc.Add(new TextItemDescriptor(this, " )"));
                return desc;
            }
        }
        public BlockDescriptor BlockDescriptor
        {
            get
            {
                BlockDescriptor desc = new BlockDescriptor();
                desc.Add(new BlockStatementDescriptor(this, "Body"));
                return desc;
            }
        }
        public string Type
        {
            get
            {
                return "WhileStatement";
            }
        }
        public bool IsClosing
        {
            get { return false; }
        }
        
    }
}
