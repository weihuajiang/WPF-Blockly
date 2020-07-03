using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public abstract class Node
    {
        protected abstract Completion ExecuteImpl(ExecutionEnvironment enviroment);
        public Completion Execute(ExecutionEnvironment environment)
        {
            ExecutionEnterEventArgs args = new ExecutionEnterEventArgs(this);
            environment.FireEnterNode(args);
            try
            {
                var c = this.ExecuteImpl(environment);
                environment.FireLeaveNode(new ExecutionLeaveEventArgs(this, c));
                return c;
            }
            catch (Exception e)
            {
                environment.FireLeaveNode(new ExecutionLeaveEventArgs(this, Completion.Exception(e.Message, this)));
                return Completion.Exception(e.Message, this);
            }
        }
    }
}
