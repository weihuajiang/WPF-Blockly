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
        public string ReturnType
        {
            get { return "void"; }
        }
        public Completion Execute(ExecutionEnvironment enviroment)
        {
            if (Try == null || Try.Body.Count == 0)
                return Completion.Void;
            ExecutionEnvironment te = new ExecutionEnvironment(enviroment);
            var c = Try.Execute(te);
            if(c.Type== CompletionType.Exception)
            {
                if(Catch!=null && Catch.Body.Count > 0)
                {
                    ExecutionEnvironment ee = new ExecutionEnvironment(enviroment);
                    c = Catch.Execute(ee);
                }
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
                return fc;
            if (fc.Type == CompletionType.Return)
                return fc;
            return c;
        }

        public Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "Try"));
                return desc;
            }
        }
        public BlockDescriptor BlockDescriptor
        {
            get
            {
                BlockDescriptor desc = new BlockDescriptor();
                desc.Add(new BlockStatementDescriptor(this, "Try"));

                Descriptor d = new ScratchNet.Descriptor();
                d.Add(new TextItemDescriptor(this, "Catch("));
                d.Add(new ParameterDescriptor(this, 0, Exception[0].Name, "exception", ParamDirection.In));
                d.Add(new TextItemDescriptor(this, ")"));
                desc.Add(new ExpressionStatementDescription(this, "catchFunc",d));

                desc.Add(new BlockStatementDescriptor(this, "Catch"));
                if (Finally != null)
                {
                    desc.Add(new TextBlockStatementDescritor(this, "Finally", "Finally"));
                    desc.Add(new BlockStatementDescriptor(this, "Finally"));
                }
                return desc;
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
