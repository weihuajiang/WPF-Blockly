using ScratchNet.Properties;
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

        public override string ReturnType
        {
            get { return "void"; }
        }
        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Test == null)
                return Completion.Exception(Language.NullException, this);
            var t = Test.Execute(enviroment);
            if (t.Type != CompletionType.Value)
                return t;
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
                return Completion.Exception(Properties.Language.NotBoolean, Test);
        }

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "if ", true));
                desc.Add(new TextItemDescriptor(this, "("));
                desc.Add(new ExpressionDescriptor(this, "Test", "boolean") { IsOnlyNumberAllowed = false });
                desc.Add(new TextItemDescriptor(this, ")"));
                return desc;
            }
        }
        public override BlockDescriptor BlockDescriptor
        {
            get
            {
                BlockDescriptor block = new BlockDescriptor();
                block.Add(new BlockStatementDescriptor(this, "Consequent"));
                if (Alternate != null)
                {
                    Descriptor d = new Descriptor();
                    d.Add(new TextItemDescriptor(this, "else", true));
                    block.Add(new ExpressionStatementDescription(this, "else", d));
                    block.Add(new BlockStatementDescriptor(this, "Alternate"));
                }
                return block;
            }
        }
        public override string Type
        {
            get
            {
                return "IfStatement";
            }
        }
        public override bool IsClosing
        {
            get { return false; }
        }

    }
}
