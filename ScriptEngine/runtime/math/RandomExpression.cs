using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class RandomExpression : Expression
    {
        public string ReturnType
        {
            get { return "number"; }
        }
        public Expression Min { get; set; }
        public Expression Max { get; set; }
        public Completion Execute(ExecutionEnvironment enviroment)
        {
            return Completion.Void;
        }

        public Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new ExpressionDescriptor(this, "Min", "number"));
                //desc.Add(new TextItemDescriptor(this, "和"));
                desc.Add(new TextItemDescriptor(this, Properties.Resources.random1));
                desc.Add(new ExpressionDescriptor(this, "Max", "number"));
                //desc.Add(new TextItemDescriptor(this, "之间的随机数"));
                desc.Add(new TextItemDescriptor(this, Properties.Resources.random2));
                return desc;
            }
        }
        public string Type
        {
            get { return "RandomExpression"; }
        }
        public bool IsClosing
        {
            get { return false; }
        }//execution  

        static Random random = new Random();
        public ExecutionEnvironment StartCall(ExecutionEnvironment e)
        {
            ExecutionEnvironment ex = new ExecutionEnvironment(e);
            ex.SetState("minimum", 0);
            ex.SetState("maximum", 0);
            ex.SetState("current", 0);
            return ex;
        }

        public Completion EndCall(ExecutionEnvironment e)
        {
            double r = random.Next((int)e.GetState("minimum"), (int)e.GetState("maximum"));
            return new Completion(r);
        }

        public bool PopStack(out object execution, out ExecutionCallback callback, ExecutionEnvironment e)
        {
            int current = (int)e.GetState("current");
            callback = Callback;
            if (current == 0)
            {
                execution = Min;
                return true;
            }
            else
            {
                execution = Max;
                return false;
            }
        }
        Nullable<DateTime> Callback(object value, object exception, ExecutionEnvironment e)
        {
            int current = (int)e.GetState("current");
            if (current == 0)
            {
                e.SetState("minimum", (int)Math.Ceiling((double)value));
            }
            else
            {
                e.SetState("maximum", (int)Math.Floor((double)value));
            }
            e.SetState("current", current + 1);
            return null;
        }
        public bool HandleException(object exception)
        {
            throw new NotImplementedException();
        }
    }

}
