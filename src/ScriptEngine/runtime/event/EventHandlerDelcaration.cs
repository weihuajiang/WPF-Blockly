using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public abstract class EventHandlerDelcaration : EventHandler
    {
        public EventHandlerDelcaration()
        {
            Params = new List<Parameter>();
            Body = new BlockStatement();
        }
        public override List<Parameter> Params { get; set; }
        public override string Name { get; set; }
        public override BlockStatement Body { get; set; }

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Body == null)
                return Completion.Void;
            var c = Body.Execute(enviroment);
            if (c.Type == CompletionType.Value || c.Type == CompletionType.Exception)
                return c;
            if (c.Type == CompletionType.Return)
                return new Completion(c.ReturnValue);
            return Completion.Void;
        }

        public override string ReturnType
        {
            get { return "void"; }
        }

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this,"event "+ Name+ "("));
                int i = 0;
                foreach (Parameter p in Params)
                {
                    if (i != 0)
                        desc.Add(new TextItemDescriptor(this, ","));
                    desc.Add(new ParameterDescriptor(this, i, p.Name, p.Type, p.Direction));
                    i++;
                }
                desc.Add(new TextItemDescriptor(this, ")"));
                return desc;
            }
        }

        public override string Type
        {
            get { return "FunctionDeclaration"; }
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
        public bool IsClosing
        {
            get { return false; }
        }


        public override Event Event
        {
            get;
            set;
        }



        public override string Format
        {
            get;
            set;
        }
    }
}
