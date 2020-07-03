using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class TryStatement:Statement
    {
        public TryStatement()
        {
            Try = new BlockStatement();
            Catch = new BlockStatement();
            Exception = new List<Parameter>();
            Parameter e = new Parameter() { Name = "e", Type = "exception" };
            Exception.Add(e);
        }
        public BlockStatement Try
        {
            get;
            set;
        }
        public BlockStatement Catch
        {
            get;
            set;
        }
        public BlockStatement Finally
        {
            get;
            set;
        }
        public List<Parameter> Exception
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
            if (Try == null || Try.Body.Count == 0)
                return Completion.Void;
            ExecutionEnvironment te = new ExecutionEnvironment(enviroment);
            var c = Try.Execute(te);
            if(c.Type== CompletionType.Exception)
            {
                ExecutionEnvironment ec = new ExecutionEnvironment(enviroment);
                ec.RegisterValue("e", c.ReturnValue);
                if (Catch != null && Catch.Body.Count > 0)
                {
                    ExecutionEnvironment ee = new ExecutionEnvironment(ec);
                    c = Catch.Execute(ee);
                }
                else
                    c = Completion.Void;
                if (c.Type == CompletionType.Exception)
                    return c;
            }
            Completion fc = Completion.Void;
            if(Finally!=null && Finally.Body.Count>0)
            {
                ExecutionEnvironment fe = new ExecutionEnvironment(enviroment);
                fc = Finally.Execute(fe);
            }
            if (c.Type == CompletionType.Return)
                return c;
            else if (c.Type == CompletionType.Value)
                return c;
            if (fc.Type == CompletionType.Return)
                return fc;
            return c;
        }

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "try", true));
                return desc;
            }
        }
        public override BlockDescriptor BlockDescriptor
        {
            get
            {
                BlockDescriptor desc = new BlockDescriptor();
                desc.Add(new BlockStatementDescriptor(this, "Try"));

                Descriptor d = new Descriptor();
                d.Add(new TextItemDescriptor(this, "catch", true));
                d.Add(new TextItemDescriptor(this, "("));
                d.Add(new ParameterDescriptor(this, 0, Exception[0].Name, "exception", ParamDirection.In));
                d.Add(new TextItemDescriptor(this, ")"));
                desc.Add(new ExpressionStatementDescription(this, "catchFunc",d));

                desc.Add(new BlockStatementDescriptor(this, "Catch"));
                if (Finally != null)
                {
                    Descriptor fd = new Descriptor();
                    fd.Add(new TextItemDescriptor(this, "finally", true));
                    desc.Add(new ExpressionStatementDescription(this, "finally", fd));
                    desc.Add(new BlockStatementDescriptor(this, "Finally"));
                }
                return desc;
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
