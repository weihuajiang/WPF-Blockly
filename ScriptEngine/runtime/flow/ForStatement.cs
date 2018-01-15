using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class ForStatement : Loop, Execution2
    {
        public ForStatement()
        {
            Body = new BlockStatement();
        }
        public Expression Loop { get; set; }
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
                desc.Add(new TextItemDescriptor(this, "Loop "));
                desc.Add(new ExpressionDescriptor(this, "Loop", "number"));
                desc.Add(new TextItemDescriptor(this, " time"));
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
            ex.SetState("loopNumber", 0);
            ex.SetState("currentNumber", 0);
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
            int currentNumber = (int)e.GetState("currentNumber");
            int loopNumber = (int)e.GetState("loopNumber");
            if (current == 0)
            {
                execution = Loop;
                callback = Callback;
                return true;
            }
            else
            {
                execution = Body;
                callback = Callback;
                return currentNumber<loopNumber;
            }
        }
        Nullable<DateTime> Callback(object value, object exception, ExecutionEnvironment e)
        {
            int current = (int)e.GetState("current");
            int currentNumber = (int)e.GetState("currentNumber");
            if (current == 0)
            {
                e.SetState("loopNumber", int.Parse(value + ""));
            }
            else
            {
                e.SetState("currentNumber", currentNumber+1);
            }
            e.SetState("current", current+1); 
            current = (int)e.GetState("current");
            e.SetState("result", value);
            return null;
        }
        public bool HandleException(object exception)
        {
            throw new NotImplementedException();
        }
    }
}
