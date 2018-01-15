using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class ExecutionEngine : IExecutionEngine
    {
        //List<Class> Classes { get; set; }
        List<Instance> Instances { get; set; }
        public ExecutionEnvironment BaseEnvironment { get; internal set; }
        List<RunThread> Threads = new List<RunThread>();
        Dictionary<RunThread, DateTime> ThreadNextRun = new Dictionary<RunThread, DateTime>();
        Thread RunThread;
        bool requestStop = false;
        AutoResetEvent resetEvent = new AutoResetEvent(true);
        public ExecutionEngine()
        {
            Instances = new List<Instance>();
            BaseEnvironment = new ExecutionEnvironment(this);
            Threads = new List<RunThread>();
        }
        public void AddInstance(Instance inst)
        {

            Lock();
            Instances.Add(inst);
            inst.States.Clear();
            foreach (Variable v in inst.Class.Variables)
            {
                inst.States.Add(v.Name, v.Value);
            }
            UnLock();
        }
        public Instance CreateInstance(Class c)
        {
            Lock();
            Instance inst = new Instance(c) ;
            Instances.Add(inst);
            foreach (Variable v in inst.Class.Variables)
            {
                inst.States.Add(v.Name, v.Value);
            }
            UnLock();
            return inst;
        }
        
        public void Start()
        {
            requestStop = false;
            RunThread = new Thread(new ThreadStart(Run));
            RunThread.Start();
        }
        public void Stop()
        {
            requestStop = true;
            if(RunThread!=null)
                RunThread.Join();
        }
        private void Run()
        {
            Nullable<DateTime> nextRun = DateTime.Now.AddMilliseconds(10);
            while (!requestStop)
            {
                Lock();

                foreach (RunThread t in Threads)
                {
                    if (ThreadNextRun.ContainsKey(t))
                    {
                        DateTime d = ThreadNextRun[t];
                        if (d < DateTime.Now)
                        {
                            ThreadNextRun.Remove(t);
                            nextRun = null;
                        }
                        else
                        {
                            if (nextRun != null && nextRun > d)
                                nextRun = d;
                            continue;
                        }
                    }
                    else
                    {
                        nextRun = null;
                    }
                    if (!t.IsCompleted)
                    {
                        Nullable<DateTime> next = t.Step();
                        if (next != null)
                        {
                            ThreadNextRun.Add(t, next.Value);
                            if (nextRun!=null && next < nextRun)
                                nextRun = next;
                        }
                        else
                        {
                            nextRun = null;
                        }
                    }
                }
                foreach (RunThread t in Threads)
                {
                    if(t.IsCompleted)
                    {
                        Threads.Remove(t);
                        break;
                    }
                }
                UnLock();
                if (nextRun != null)
                {
                    Thread.Sleep(10);
                }
            }
        }
        public void SendEvent(Event e)
        {
            Lock();
            foreach (Instance inst in Instances)
            {
                foreach (EventHandler func in inst.Class.Handlers)
                {
                    if (func.IsProcessEvent(e))
                    {
                        Threads.Add(new RunThread(inst, func, e,BaseEnvironment));
                    }
                }
            }
            UnLock();
        }
        public void SendEventAsych(Event e)
        {
            new Thread(new ThreadStart(() =>
            {
                SendEvent(e);
            })).Start();
        }
        public void SendEventAsych(Instance inst, Event e)
        {
            new Thread(new ThreadStart(() =>
            {
                SendEvent(inst, e);
            })).Start();
        }
        public void SendEvent(Instance inst, Event e){
            Lock();
            foreach (EventHandler func in inst.Class.Handlers)
            {
                if (func.IsProcessEvent(e))
                {
                    Threads.Add(new RunThread(inst, func, e, BaseEnvironment));
                }
            }
            UnLock();
        }
        public void Lock()
        {
            resetEvent.WaitOne();
        }
        public void UnLock()
        {
            resetEvent.Set();
        }
    }
}
