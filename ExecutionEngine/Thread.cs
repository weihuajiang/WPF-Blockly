using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class RunThread
    {
        public Instance Instance { get; internal set; }
        public ExecutionEnvironment Environment { get; internal set; }
        public bool IsStarted { get; internal set; }
        public bool IsCompleted { get; internal set; }
        Stack<ExecStackItem> Stacks { get; set; }
        public RunThread(Instance instance, EventHandler fun, Event e, ExecutionEnvironment environment)
        {
            Instance = instance;
            fun.Event = e;
            IsStarted = false;
            IsCompleted = false;
            Stacks = new Stack<ExecStackItem>();
            Execution2 exe = fun as Execution2;
            Environment = new ExecutionEnvironment(environment, instance);
            
            ExecutionEnvironment env= exe.StartCall(Environment);
            Stacks.Push(new ExecStackItem(exe, FinishCallback, env, Environment));
        }
        Nullable<DateTime> FinishCallback(object value, object exception, ExecutionEnvironment e)
        {
            IsCompleted = true;
            return null;
        }
        public Nullable<DateTime> Step()
        {
            IsStarted = true;
            if (Stacks.Count == 0)
            {
                IsCompleted = true;
                return null;
            }
            ExecStackItem current = Stacks.Peek();
            if (!current.HasMoreExecution)
            {
                return EndCall(current);
            }
            object execution;
            ExecutionCallback callback;
            current.HasMoreExecution = (current.Execution.PopStack(out execution, out callback, current.Environment));
            //Console.WriteLine("current is " + execution);
            if (execution is Execution2)
            {
                    Execution2 exec = execution as Execution2;
                    ExecutionEnvironment e= exec.StartCall(current.Environment);
                    Stacks.Push(new ExecStackItem(exec, callback, e, current.Environment));
            }
            else
            {
                return EndCall(current);
            }
            
            return null;
        }
        void HandleException(Exception e)
        {
            Console.WriteLine("Exception occur");
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
            ExecutionCallback callback = null;
            while (Stacks.Count > 0)
            {
                ExecStackItem item = Stacks.Peek();
                bool handle = false;
                try
                {
                    handle=item.Execution.HandleException(e);
                    callback(null, e, item.Previous);
                }
                catch (Exception excption) { }
                if (handle)
                {
                    return;
                }
                else
                {
                    Stacks.Pop();
                    callback = item.Callback;
                }
            }
            Console.WriteLine("Exception occur and no try block, so exit");
            //TO DO terminate execution
        }
        Nullable<DateTime> ExecCallBack(ExecutionCallback callback, object value, object exception, ExecutionEnvironment env)
        {
            Exception ex = null;
            try
            {
               return callback(value, exception, env);
            }
            catch (Exception e) {
                ex = e;
            }
            if(ex!=null)
                HandleException(ex);
            return null;
        }
        Nullable<DateTime> EndCall(ExecStackItem current)
        {
            Exception ex = null;
            Completion retVal = null;
            try
            {
                retVal = current.Execution.EndCall(current.Environment);
            }
            catch (Exception e)
            {
                ex = e;
            }
            if (ex != null)
            {
                HandleException(ex);
                return null;
            }
            Stacks.Pop();
            if (current.Execution is ReturnStatement)
            {
                ExecStackItem func = Stacks.Peek();
                while (!(func.Execution is Function))
                {
                    Stacks.Pop();
                    func = Stacks.Peek();
                }
                ExecCallBack(func.Callback,retVal.ReturnValue, null, func.Previous);
                return null;
            }
            else if (current.Execution is BreakStatement)
            {
                ExecStackItem func = Stacks.Peek();
                while (!(func.Execution is Loop))
                {
                    Stacks.Pop();
                    func = Stacks.Peek();
                    if (Stacks.Count == 1)
                        break;
                }
                func = Stacks.Pop();
                if(!(func.Execution is Loop))
                {
                    HandleException(new Exception("Break must be in a loop"));
                    return null;
                }
                ExecCallBack(func.Callback, retVal.ReturnValue, null, func.Previous);
                return null;
            }
            else if (current.Execution is ContinueStatement)
            {
                ExecStackItem func = Stacks.Peek();
                ExecStackItem last=null;
                ExecutionCallback callBack = null;
                while (!(func.Execution is Loop))
                {
                    callBack = func.Callback;
                    last = func;
                    Stacks.Pop();
                    func = Stacks.Peek();
                    if (Stacks.Count == 1)
                        break;
                }
                if (!(func.Execution is Loop))
                {
                    HandleException(new Exception("continue must be in a loop"));
                    return null;
                }
                return ExecCallBack(last.Callback, retVal.ReturnValue, null, last.Environment);
            }
            return ExecCallBack(current.Callback, retVal.ReturnValue, null, current.Previous);
        }
    }
    class ExecStackItem
    {
        public ExecStackItem(Execution2 exec, ExecutionCallback callback, ExecutionEnvironment env, ExecutionEnvironment preEnv)
        {
            Execution = exec;
            Callback = callback;
            Environment = env;
            HasMoreExecution = true;
            Previous = preEnv;
        }
        public Execution2 Execution { get; set; }
        public ExecutionCallback Callback { get; set; }
        public ExecutionEnvironment Environment { get; set; }
        public ExecutionEnvironment Previous { get; set; }
        public bool HasMoreExecution { get; set; }
    }
}
