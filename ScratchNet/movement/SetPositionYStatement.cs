using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class SetPositionYStatement : Statement
    {
        public SetPositionYStatement()
        {
            Y = new Literal() { Raw = "5" };
        }
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
                desc.Add(new TextItemDescriptor(this, Localize.GetString("cs_Y_CoordinatesSetTo")));
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

        double y = 0;
        public ExecutionEnvironment StartCall(ExecutionEnvironment e)
        {
            y = 0;
            return e;
        }
        public Completion EndCall(ExecutionEnvironment _env)
        {
            Sprite sp = (_env.This as Instance).Class as Sprite;
            ((_env.This as Instance).Class as Sprite).Y = (int)y;
            App.Current.Dispatcher.InvokeAsync(new Action(() =>
            {
                (_env.GetValue("$$Player") as ScriptPlayer).DrawScript();
            }));

            return Completion.Void;
        }

        public bool PopStack(out object execution, out ExecutionCallback callback, ExecutionEnvironment e)
        {
                execution = Y;
                callback = Callback;
                return false;
        }
        Nullable<DateTime> Callback(object value, object exception, ExecutionEnvironment e)
        {
                y = (double)value;
            return null;
        }
        public bool HandleException(object exception)
        {
            throw new NotImplementedException();
        }
    }
}
