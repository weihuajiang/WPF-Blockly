using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class IfStatement : Statement, Execution2
    {
        public IfStatement()
        {
            Consequent = new BlockStatement();
        }
        public Expression Test { get; set; }
        public BlockStatement Consequent
        {
            get;
            set;
        }
        public BlockStatement Alternate { get; set; }
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
                desc.Add(new TextItemDescriptor(this, "If "));
                desc.Add(new ExpressionDescriptor(this, "Test", "boolean"));
                desc.Add(new TextItemDescriptor(this, " then"));
                return desc;
            }
        }
        public BlockDescriptor BlockDescriptor
        {
            get
            {
                BlockDescriptor desc = new BlockDescriptor();
                desc.Add(new BlockStatementDescriptor(this, "Consequent"));
                if (Alternate != null)
                {
                    desc.Add(new TextBlockStatementDescritor(this, "Alternate", "Else"));
                    desc.Add(new BlockStatementDescriptor(this, "Alternate"));
                }
                return desc;
            }
        }
        public string Type
        {
            get
            {
                return "IfStatement";
            }
        }
        public bool IsClosing
        {
            get { return false; }
        }

        public ExecutionEnvironment StartCall(ExecutionEnvironment e)
        {
            ExecutionEnvironment ex = new ExecutionEnvironment(e);
            ex.SetState("current", 0);
            ex.SetState("result", null);
            ex.SetState("testResult", false);
            return ex;
        }

        public Completion EndCall(ExecutionEnvironment e)
        {
            return new Completion(e.GetState("result"));
        }

        public bool PopStack(out object execution, out ExecutionCallback callback, ExecutionEnvironment e)
        {
            int current = (int)e.GetState("current");
            bool testResult = (bool)e.GetState("testResult");
            if (current == 0)
            {
                execution = Test;
                callback = Callback;
                return true;
            }
            else if(current==1)
            {
                if (testResult)
                {
                    execution = Consequent;
                }
                else
                    execution = Alternate;
                callback = Callback;
                return false;
            }
            execution = null;
            callback = null;
            return false;
        }
        Nullable<DateTime> Callback(object value, object exception, ExecutionEnvironment e)
        {
            int current = (int)e.GetState("current");
            if (current == 0)
            {
                 e.SetState("testResult",value);
            }
            else
            {
            }
            current++;
            e.SetState("current", current);
            e.SetState("result", value);
            return null;
        }
        public bool HandleException(object exception)
        {
            throw new NotImplementedException();
        }
    }
}
