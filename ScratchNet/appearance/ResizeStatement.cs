using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class ResizeStatement : Statement
    {
        public ResizeStatement()
        {
            Size = new Literal() { Raw = "90" };
        }
        public Expression Size { get; set; }
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
                desc.Add(new TextItemDescriptor(this, "将大小设为"));
                desc.Add(new ExpressionDescriptor(this, "Size", "number"));
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
        
        int size = 100;

        public ExecutionEnvironment StartCall(ExecutionEnvironment e)
        {
            size = 100;
            return e;
        }
        public Completion EndCall(ExecutionEnvironment _env)
        {
            ((_env.This as Instance).Class as Sprite).Size = size;
            App.Current.Dispatcher.InvokeAsync(new Action(() =>
            {
                (_env.GetValue("$$Player") as ScriptPlayer).DrawScript();
            }));
            object result = size;
            return new Completion(result);
        }

        public bool PopStack(out object execution, out ExecutionCallback callback, ExecutionEnvironment e)
        {
            execution = Size;
            callback = Callback;
            return false;
        }
        Nullable<DateTime> Callback(object value, object exception, ExecutionEnvironment e)
        {
            size = int.Parse(value + "");
            return null;
        }
        public bool HandleException(object exception)
        {
            throw new NotImplementedException();
        }
    }
}
