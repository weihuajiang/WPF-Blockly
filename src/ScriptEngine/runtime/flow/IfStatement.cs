using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class IfStatement : Statement
    {
        public IfStatement()
        {
            Consequent = new BlockStatement();
        }
        public Expression Test { get; set; }
        public BlockStatement Consequent
        {
            get;
            set;
        }
        BlockStatement _alternate;
        public BlockStatement Alternate { get
            {
                return _alternate;
            }
            set
            {
                _alternate = value;
            }
        }

        public string ReturnType
        {
            get { return "void"; }
        }
        public Completion Execute(ExecutionEnvironment enviroment)
        {
            if (Test == null)
                return Completion.Void;
            var t = Test.Execute(enviroment);
            if (t.ReturnValue is bool)
            {
                if ((bool)t.ReturnValue)
                {
                    if (Consequent == null)
                        return Completion.Void;
                    ExecutionEnvironment current = new ExecutionEnvironment(enviroment);
                    return Consequent.Execute(current);
                }
                else
                {
                    if (Alternate == null)
                        return Completion.Void;
                    ExecutionEnvironment current = new ExecutionEnvironment(enviroment);
                    return Alternate.Execute(current);
                }
            }
            else
                return new Completion("Test return not boolean value", CompletionType.Exception);
        }

        public Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "If "));
                desc.Add(new ExpressionDescriptor(this, "Test", "boolean"));
                desc.Add(new TextItemDescriptor(this, " then"));
                return desc;
            }
        }
        public BlockDescriptor BlockDescriptor
        {
            get
            {
                BlockDescriptor block = new BlockDescriptor();
                block.Add(new BlockStatementDescriptor(this, "Consequent"));
                if (Alternate != null)
                {
                    block.Add(new TextBlockStatementDescritor(this, "Alternate", "Else"));
                    block.Add(new BlockStatementDescriptor(this, "Alternate"));
                }
                return block;
            }
        }
        public string Type
        {
            get
            {
                return "IfStatement";
            }
        }
        public bool IsClosing
        {
            get { return false; }
        }

    }
}
