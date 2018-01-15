using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class MoveStatement : Statement
    {
        public MoveStatement()
        {
            Step = new Literal() { Raw = "5" };
        }
        public Expression Step { get; set; }
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
                desc.Add(new TextItemDescriptor(this, "移动"));
                desc.Add(new ExpressionDescriptor(this, "Step", "number"));
                desc.Add(new TextItemDescriptor(this, "步"));
                return desc;
            }
        }
        public string Type
        {
            get
            {
                return "MoveStatement";
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
        double degree = 0;

        public ExecutionEnvironment StartCall(ExecutionEnvironment e)
        {
            degree = 0;
            return e;
        }
        public Completion EndCall(ExecutionEnvironment _env)
        {
            bool reflection = false;
            if (_env.This.States.ContainsKey("$$ReflectionOnTouchSide&&") && (bool)_env.This.States["$$ReflectionOnTouchSide&&"])
                reflection = true;
            Sprite sp = (_env.This as Instance).Class as Sprite;
            int x=sp.X + (int)(Math.Cos(sp.Direction*Math.PI/180) * degree);
            int y=sp.Y + (int)(Math.Sin(sp.Direction * Math.PI / 180) * degree);
            int direction = sp.Direction;
            if (reflection)
            {
                while (x < 0)
                {
                    x = -x;
                    direction = 180 - direction;
                }
                while (x > CurrentEnviroment.ScreenWidth)
                {
                    x = CurrentEnviroment.ScreenWidth * 2 - x;
                    direction = 180 - direction;
                }
                while (y < 0)
                {
                    y = -y;
                    direction = -direction;
                }
                while (y > CurrentEnviroment.ScreenHeight)
                {
                    y = CurrentEnviroment.ScreenHeight * 2 - y;
                    direction = -direction;
                }
            }
            sp.X = x;
            sp.Y = y;
            sp.Direction = direction;
            App.Current.Dispatcher.InvokeAsync(new Action(() =>
            {
                (_env.GetValue("$$Player") as ScriptPlayer).DrawScript();
            }));
            return Completion.Void;
        }

        public bool PopStack(out object execution, out ExecutionCallback callback, ExecutionEnvironment e)
        {
            execution = Step;
            callback = Callback;
            return false;
        }
        Nullable<DateTime> Callback(object value, object exception, ExecutionEnvironment e)
        {
            degree = (double)value;
            return null;
        }
        public bool HandleException(object exception)
        {
            throw new NotImplementedException();
        }
    }
}
