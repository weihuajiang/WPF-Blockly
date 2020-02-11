using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class TimedMoveStatement : Statement
    {
        public TimedMoveStatement()
        {
            Time = new Literal() { Raw = "2" };
        }
        public Expression Time { get; set; }
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
                desc.Add(new TextItemDescriptor(this, Localize.GetString("cs_In")));
                desc.Add(new ExpressionDescriptor(this, "Time", "number"));
                desc.Add(new TextItemDescriptor(this, Localize.GetString("cs_MoveXinSec")));
                desc.Add(new ExpressionDescriptor(this, "X", "number"));
                desc.Add(new TextItemDescriptor(this, ", Y "));
                desc.Add(new ExpressionDescriptor(this, "Y", "number"));
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

        double current = 0;
        double time;
        double x;
        double y;
        DateTime startTime;
        DateTime finishTime;
        bool finish = false;

        public ExecutionEnvironment StartCall(ExecutionEnvironment e)
        {
            current = 0;
            time = 0;
            x = 0;
            y = 0;
            startTime = DateTime.Now;
            finish = false;
            return e;
        }
        public Completion EndCall(ExecutionEnvironment _env)
        {
            Sprite sp = (_env.This as Instance).Class as Sprite;
            sp.X = (int)x;
            sp.Y = (int)y;
            App.Current.Dispatcher.InvokeAsync(new Action(() =>
            {
                (_env.GetValue("$$Player") as ScriptPlayer).DrawScript();
            }));
            return Completion.Void;
        }

        public bool PopStack(out object execution, out ExecutionCallback callback, ExecutionEnvironment e)
        {
            callback = Callback;
            if (current == 0)
            {
                execution = Time;
                return true;
            }
            else if (current == 1)
            {
                execution = X;
                return true;
            }
            else if (current == 2)
            {
                execution = Y;
                return true;
            }
            else if(!finish)
            {
                execution = new Literal() { Raw="1" };
                return true;
            }
            execution = null;
            return false;
        }
        double orgin_x;
        double orgin_y;
        double totalTime;
        Nullable<DateTime> Callback(object value, object exception, ExecutionEnvironment _env)
        {
            if (current == 0)
            {
                time = (double)value;
                current++;
                return null;
            }
            else if (current == 1)
            {
                x = (double)value;
                current++;
                return null;
            }
            else if (current == 2)
            {
                y = (double)value;
                Sprite sp = (_env.This as Instance).Class as Sprite;
                if (x < 0)
                    x = 0;
                if (x > CurrentEnviroment.ScreenWidth)
                    x = CurrentEnviroment.ScreenWidth;
                if (y < 0)
                    y = 0;
                if (y > CurrentEnviroment.ScreenHeight)
                    y = CurrentEnviroment.ScreenHeight;
                orgin_x = sp.X;
                orgin_y = sp.Y;
                startTime = DateTime.Now;
                finishTime = DateTime.Now.AddSeconds(time);
                current++;
                if (finishTime <= startTime)
                {
                    finish = true;
                    return null;
                }
                totalTime = (finishTime - startTime).TotalMilliseconds;
                return DateTime.Now.AddMilliseconds(20);
            }
            else
            {
                current++;
                if (DateTime.Now > finishTime)
                {
                    finish = true;
                    return null;
                }
                double duration = (DateTime.Now - startTime).TotalMilliseconds;

                Sprite sp = (_env.This as Instance).Class as Sprite;
                sp.X = (int)(orgin_x + (x - orgin_x) * duration / totalTime);
                sp.Y = (int)(orgin_y + (y - orgin_y) * duration / totalTime);
                App.Current.Dispatcher.InvokeAsync(new Action(() =>
                {
                    (_env.GetValue("$$Player") as ScriptPlayer).DrawScript();
                }));
                return DateTime.Now.AddMilliseconds(20);
            }
            return null;
        }
        public bool HandleException(object exception)
        {
            throw new NotImplementedException();
        }
    }
}
