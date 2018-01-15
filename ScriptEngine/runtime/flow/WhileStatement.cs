using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{

    public class WhileStatement : Loop, Execution2
    {
        public WhileStatement()
        {
            Body = new BlockStatement();
        }
        public Expression Test { get; set; }
        public BlockStatement Body
        {
            get;
            set;
        }
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
                desc.Add(new TextItemDescriptor(this, "While("));
                desc.Add(new ExpressionDescriptor(this, "Test", "boolean"));
                desc.Add(new TextItemDescriptor(this, " )"));
                return desc;
            }
        }
        public BlockDescriptor BlockDescriptor
        {
            get
            {
                BlockDescriptor desc = new BlockDescriptor();
                desc.Add(new BlockStatementDescriptor(this, "Body"));
                return desc;
            }
        }
        public string Type
        {
            get
            {
                return "WhileStatement";
            }
        }
        public bool IsClosing
        {
            get { return false; }
        }
        //execution
        public ExecutionEnvironment StartCall(ExecutionEnvironment e)
        {
            ExecutionEnvironment ex = new ExecutionEnvironment(e);
            ex.SetState("canContinue", false);
            ex.SetState("current", 0);
            ex.SetState("result", null);
            return ex;
        }

        public Completion EndCall(ExecutionEnvironment e)
        {
            return new Completion(e.GetState("result"));
        }

        public bool PopStack(out object execution, out ExecutionCallback callback, ExecutionEnvironment e)
        {
            int current = (int)e.GetState("current");
            bool canContinue = (bool)e.GetState("canContinue");
            if (current %2 == 0)
            {
                execution = Test;
                callback = Callback;
                return true;
            }
            else if(canContinue)
            {
                execution = Body;
                callback = Callback;
                return true;
            }
            execution = null;
            callback = null;
            return false;
        }
        Nullable<DateTime> Callback(object value, object exception, ExecutionEnvironment e)
        {
            int current = (int)e.GetState("current");
            if (current%2 == 0)
            {
                e.SetState("canContinue", value);
            }
            else
            {
                e.SetState("result", value);
            }
            current++;
            e.SetState("current", current);
            return null;
        }
        public bool HandleException(object exception)
        {
            throw new NotImplementedException();
        }
    }
}
