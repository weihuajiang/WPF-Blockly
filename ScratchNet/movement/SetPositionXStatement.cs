using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class SetPositionXStatement : Statement
    {
        public SetPositionXStatement()
        {
            X = new Literal() { Raw = "5" };
        }
        public Expression X { get; set; }
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
                desc.Add(new TextItemDescriptor(this, "X坐标设置为 "));
                desc.Add(new ExpressionDescriptor(this, "X", "number"));
                return desc;
            }
        }
        public string Type
        {
            get
            {
                return "SetPositionStatement";
            }
        }


        public BlockDescriptor BlockDescriptor
        {
            get { return null; }
        }


        public bool IsClosing
        {
            get { return false; }
        }//execution  

        double x = 0;

        public ExecutionEnvironment StartCall(ExecutionEnvironment e)
        {
            x = 0;
            return e;
        }
        public Completion EndCall(ExecutionEnvironment _env)
        {
            Sprite sp = (_env.This as Instance).Class as Sprite;
            ((_env.This as Instance).Class as Sprite).X = (int)x;
            App.Current.Dispatcher.InvokeAsync(new Action(() =>
            {
                (_env.GetValue("$$Player") as ScriptPlayer).DrawScript();
            }));

            return Completion.Void;
        }

        public bool PopStack(out object execution, out ExecutionCallback callback, ExecutionEnvironment e)
        {
                execution = X;
                callback = Callback;
                return false;
        }
        Nullable<DateTime> Callback(object value, object exception, ExecutionEnvironment e)
        {
                x = (double)value;
            return null;
        }
        public bool HandleException(object exception)
        {
            throw new NotImplementedException();
        }
    }
}
