using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{

    //定义代码块和flow control， 比如if, while
    public class BlockStatement : Execution2
    {
        ObservableCollection<Statement> _body = new ObservableCollection<Statement>();
        public ObservableCollection<Statement> Body
        {
            get { return _body; }
            set
            {
                _body = value;
            }
        }

        public ExecutionEnvironment StartCall(ExecutionEnvironment e)
        {
            ExecutionEnvironment ex = new ExecutionEnvironment(e);
            ex.SetState("current", 0);
            ex.SetState("retVal", null);
            return ex;
        }

        public Completion EndCall(ExecutionEnvironment e)
        {
            return new Completion(e.GetState("retVal"));
        }

        public bool PopStack(out object execution, out ExecutionCallback callback, ExecutionEnvironment e)
        {
            callback = Callback;
            int current = (int)e.GetState("current");
            if (current < Body.Count)
            {
                execution = Body[current];
                if (execution is ReturnStatement)
                {
                    return false;
                }
            }
            else
            {
                execution = null;
                return false;
            }
            return (current + 1) != Body.Count;
        }
        Nullable<DateTime> Callback(object value, object exception, ExecutionEnvironment e)
        {
            int current = (int)e.GetState("current");
            current++;
            if (current == Body.Count)
                e.SetState("retVal", value);
            e.SetState("current", current);
            return null;
        }

        public bool HandleException(object exception)
        {
            throw new NotImplementedException();
        }
    }
}
