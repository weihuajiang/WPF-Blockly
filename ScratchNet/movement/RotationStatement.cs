using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class RotationStatement : Statement
    {
        public Expression Degree { get; set; }
        public RotationDirection Direction { get; set; }
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
                if (Direction == RotationDirection.CounterClockwise)
                {
                    desc.Add(new TextItemDescriptor(this, "向左"));
                }
                else
                    desc.Add(new TextItemDescriptor(this, "向右"));
                desc.Add(new TextItemDescriptor(this, "旋转"));
                desc.Add(new ExpressionDescriptor(this, "Degree", "number"));
                desc.Add(new TextItemDescriptor(this, "度"));
                return desc;
            }
        }
        public string Type
        {
            get
            {
                return "RotationStatement";
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

        double degree = 0;

        public ExecutionEnvironment StartCall(ExecutionEnvironment e)
        {
            degree = 0;
            return e;
        }
        public Completion EndCall(ExecutionEnvironment _env)
        {
            if(Direction==RotationDirection.Clockwise)
                ((_env.This as Instance).Class as Sprite).Direction +=(int)degree;
            else
                ((_env.This as Instance).Class as Sprite).Direction -= (int)degree;
            App.Current.Dispatcher.InvokeAsync(new Action(() =>
            {
                (_env.GetValue("$$Player") as ScriptPlayer).DrawScript();
            }));
            double result = ((_env.This as Instance).Class as Sprite).Direction;
            return new Completion(result);
        }

        public bool PopStack(out object execution, out ExecutionCallback callback, ExecutionEnvironment e)
        {
            execution = Degree;
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
    public enum RotationDirection
    {
        Clockwise,
        CounterClockwise
    }
}
