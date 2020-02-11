using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class HideStatement : Statement
    {
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
                desc.Add(new TextItemDescriptor(this, Localize.GetString("a_Hide")));
                return desc;
            }
        }
        public string Type
        {
            get
            {
                return "ShowStatement";
            }
        }


        public BlockDescriptor BlockDescriptor
        {
            get { return null; }
        }


        public bool IsClosing
        {
            get { return false; }
        }
        //execution  

        public ExecutionEnvironment StartCall(ExecutionEnvironment e)
        {
            return e;
        }
        public Completion EndCall(ExecutionEnvironment _env)
        {
            Sprite sp = (_env.This as Instance).Class as Sprite;
            if (sp.Visible)
            {
                ((_env.This as Instance).Class as Sprite).Visible = false; ;
                App.Current.Dispatcher.InvokeAsync(new Action(() =>
                {
                    (_env.GetValue("$$Player") as ScriptPlayer).DrawScript();
                }));
            }
            return Completion.Void;
        }

        public bool PopStack(out object execution, out ExecutionCallback callback,ExecutionEnvironment e)
        {
            execution = null;
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
