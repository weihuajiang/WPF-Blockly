using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class LoopStatement : Loop
    {
        public LoopStatement()
        {
            Body = new BlockStatement();
        }
        public Expression Loop { get; set; }
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
            if (Loop == null || Body == null || Body.Body.Count == 0)
                return Completion.Void;
            var c = Loop.Execute(enviroment);
            if(c.Type== CompletionType.Value)
            {

                int cycle = 0;
                try
                {
                    cycle = TypeConverters.GetValue<int>(c.ReturnValue);
                }
                catch
                {
                    return Completion.Exception("loop parameter is not a number", Loop);
                }
                for(int i = 0; i < cycle; i++)
                {
                    ExecutionEnvironment current = new ExecutionEnvironment(enviroment);
                    var cx = Body.Execute(current);
                    if (cx.Type == CompletionType.Exception )
                    {
                        return cx;
                    }
                    if (cx.Type == CompletionType.Break)
                    {
                        return Completion.Void;
                    }
                    if (cx.Type == CompletionType.Return)
                        return cx;
                }
            }
            return c;
        }

        public Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "Loop "));
                desc.Add(new ExpressionDescriptor(this, "Loop", "number"));
                desc.Add(new TextItemDescriptor(this, " time"));
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
