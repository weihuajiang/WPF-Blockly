using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class SetNextImageStatement : Statement
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
                desc.Add(new TextItemDescriptor(this, "显示下一个图片"));
                return desc;
            }
        }
        public string Type
        {
            get
            {
                return "SetNextImageStatement";
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
            int current = sp.CurrentImage;
            current++;
            if (current == sp.Images.Count)
                current = 0;
            sp.CurrentImage = current;
            App.Current.Dispatcher.InvokeAsync(new Action(() =>
            {
                (_env.GetValue("$$Player") as ScriptPlayer).DrawScript();
            }));
            return Completion.Void;
        }

        public bool PopStack(out object execution, out ExecutionCallback callback, ExecutionEnvironment e)
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
