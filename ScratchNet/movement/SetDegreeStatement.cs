using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class SetDegreeStatement : Statement
    {
        public SetDegreeStatement()
        {
            Degree = new Literal() { Raw = "0" };
        }
        public Expression Degree { get; set; }
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
                desc.Add(new TextItemDescriptor(this, Localize.GetString("cs_SetDirection")));
                desc.Add(new ExpressionDescriptor(this, "Degree", "number"));
                desc.Add(new TextItemDescriptor(this, Localize.GetString("cs_Degree")));
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
            Sprite sp = (_env.This as Instance).Class as Sprite;
            if (sp.Direction != (int)degree)
            {
                ((_env.This as Instance).Class as Sprite).Direction = (int)degree;
                App.Current.Dispatcher.InvokeAsync(new Action(() =>
                {
                    (_env.GetValue("$$Player") as ScriptPlayer).DrawScript();
                }));
            }
            
            return new Completion(degree);
        }

        public bool PopStack(out object execution, out ExecutionCallback callback,ExecutionEnvironment e)
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
}
