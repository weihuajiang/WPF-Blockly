using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class SetImageStatement : Statement
    {
        public SetImageStatement()
        {
        }
        public Expression ImageIndex { get; set; }
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
                desc.Add(new TextItemDescriptor(this, "将图片为"));
                desc.Add(new SelectionExpressionItemDescriptor(this, "ImageIndex", CurrentEnviroment.CurrentSpriteImages,
                    new ExpressionDescriptor(this, "ImageIndex", "number")));
                return desc;
            }
        }
        public string Type
        {
            get
            {
                return "ResizeStatement";
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

        int currentIndex = 0;

        public ExecutionEnvironment StartCall(ExecutionEnvironment e)
        {
            currentIndex = 0;
            return e;
        }
        public Completion EndCall(ExecutionEnvironment e)
        {
            object result = currentIndex;
            return new Completion(result);
        }

        public bool PopStack(out object execution, out ExecutionCallback callback, ExecutionEnvironment e)
        {
            execution = ImageIndex;
            callback = Callback;
            return false;
        }
        Nullable<DateTime> Callback(object value, object exception, ExecutionEnvironment _env)
        {
            Sprite sp=(_env.This as Instance).Class as Sprite;
            if (value is double)
            {
                currentIndex = int.Parse(value + "");
            }
            else
            {
                string name = value + "";
                int i=0;
                foreach (Resource r in sp.Images)
                {
                    if (r.DisplayName == name)
                        currentIndex = i;
                    i++;
                }
            }
            if (currentIndex < 0)
                currentIndex = 0;
            if (currentIndex >= sp.Images.Count)
                currentIndex = 0;
            sp.CurrentImage = currentIndex;
            App.Current.Dispatcher.InvokeAsync(new Action(() =>
            {
                (_env.GetValue("$$Player") as ScriptPlayer).DrawScript();
            }));
            return null;
        }
        public bool HandleException(object exception)
        {
            throw new NotImplementedException();
        }
    }
}
