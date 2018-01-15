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
        public List<Parameter> Params { get; set; }
        public string Name { get; set; }
        public BlockStatement Body { get; set; }

        public virtual Completion Execute(ExecutionEnvironment enviroment)
        {
            throw new NotImplementedException();
        }

        public string ReturnType
        {
            get { return "void"; }
        }

        public virtual Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, Name+ "("));
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

        public virtual string Type
        {
            get { return "FunctionDeclaration"; }
        }


        public virtual BlockDescriptor BlockDescriptor
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


        public Event Event
        {
            get;
            set;
        }

        public abstract bool IsProcessEvent(Event e);


        public string Format
        {
            get;
            set;
        }


        public ExecutionEnvironment StartCall(ExecutionEnvironment e)
        {
            return e;
        }

        public Completion EndCall(ExecutionEnvironment e)
        {
            return Completion.Void;
        }

        public bool PopStack(out object execution, out ExecutionCallback callback, ExecutionEnvironment e)
        {
            execution = Body;
            callback = Callback;
            return false;
        }
        Nullable<DateTime> Callback(object value, object exception, ExecutionEnvironment e)
        {
            return null;
        }
        public bool HandleException(object exception)
        {
            throw new NotImplementedException();
        }
    }
}
