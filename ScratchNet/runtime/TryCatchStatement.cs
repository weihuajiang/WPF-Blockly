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
            return Completion.Void;
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
    }
}
