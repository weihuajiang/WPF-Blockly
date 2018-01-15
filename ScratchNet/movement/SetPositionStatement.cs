using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class SetPositionStatement : Statement
    {
        public SetPositionStatement()
        {
            X = new Literal() { Raw = "5" };
            Y = new Literal() { Raw = "5" };
        }
        public Expression X { get; set; }
        public Expression Y { get; set; }
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
                desc.Add(new TextItemDescriptor(this, "坐标设置为x "));
                desc.Add(new ExpressionDescriptor(this, "X", "number"));
                desc.Add(new TextItemDescriptor(this, ", y ")); 
                desc.Add(new ExpressionDescriptor(this, "Y", "number"));
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
        double y = 0;
        int current = 0;
        public ExecutionEnvironment StartCall(ExecutionEnvironment e)
        {
            x = 0;
            y = 0;
            current = 0;
            return e;
        }
        public Completion EndCall(ExecutionEnvironment _env)
        {
            Sprite sp = (_env.This as Instance).Class as Sprite;
            ((_env.This as Instance).Class as Sprite).X = (int)x;
            ((_env.This as Instance).Class as Sprite).Y = (int)y;
            App.Current.Dispatcher.InvokeAsync(new Action(() =>
            {
                (_env.GetValue("$$Player") as ScriptPlayer).DrawScript();
            }));

            return Completion.Void;
        }

        public bool PopStack(out object execution, out ExecutionCallback callback, ExecutionEnvironment e)
        {
            if (current == 0)
            {
                execution = X;
                callback = Callback;
                return true;
            }
            else
            {
                execution = Y;
                callback = Callback;
                return false;
            }
        }
        Nullable<DateTime> Callback(object value, object exception, ExecutionEnvironment e)
        {
            if (current == 0)
                x = (double)value;
            else
                y = (double)value;
            current++;
            return null;
        }
        public bool HandleException(object exception)
        {
            throw new NotImplementedException();
        }
    }
}
