using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScratchNet
{
    public delegate void ExecutionEnvironmentEventHandler<T>(object engine, T arg);
    public class ExecutionEnvironment
    {
        public static int BaseLevel = 1;
        public static int ClassLevel = 2;
        public static int InstanceLevel = 3;
        public static int FunctionLevel = 4;
        /// <summary>
        /// get base environment for class execution
        /// </summary>
        /// <returns></returns>
        public ExecutionEnvironment GetBaseEnvironment()
        {
            ExecutionEnvironment env = this;
            while (env.Level != BaseLevel)
                env = env.Parent;
            return env;
        }
        /// <summary>
        /// get class environment for function execution
        /// </summary>
        /// <returns></returns>
        public ExecutionEnvironment GetClassEnvironment()
        {
            ExecutionEnvironment env = this;
            if (env.Level < ClassLevel)
            {
                return new ExecutionEnvironment(this);
            }
            while (env.Level != ClassLevel)
                env = env.Parent;
            return env;
        }

        public ExecutionEnvironment GetInstanceEnvironment()
        {
            ExecutionEnvironment env = this;
            if (env.Level < InstanceLevel)
            {
                return new ExecutionEnvironment(this);
            }
            while (env.Level != InstanceLevel)
                env = env.Parent;
            return env;
        }
        /// <summary>
        /// get function environment
        /// </summary>
        /// <returns></returns>
        public ExecutionEnvironment GetFunctionEnvironment()
        {
            ExecutionEnvironment env = this;
            if (env.Level < FunctionLevel)
                return null;
            while (env.Level != FunctionLevel)
                env = env.Parent;
            return env;
        }

        public event ExecutionEnvironmentEventHandler<object> ExecutionCompleted;
        public event ExecutionEnvironmentEventHandler<object> ExecutionStarted;
        public event ExecutionEnvironmentEventHandler<Completion> ExecutionAborted;

        public event ExecutionEnvironmentEventHandler<ExecutionEnterEventArgs> EnterNode;
        public event ExecutionEnvironmentEventHandler<ExecutionLeaveEventArgs> LeaveNode;

        public bool IsCompleted { get; internal set; } = false;
        
        bool IsAborting { get; set; } = false;

        internal void FireEnterNode(ExecutionEnterEventArgs args)
        {
            if (IsAborting)
                throw new ExecutionAbortException(Properties.Language.ExecutionAborted);
            EnterNode?.Invoke(this, args);
            if (_parent != null)
                _parent.FireEnterNode(args);
        }
        internal void FireLeaveNode(ExecutionLeaveEventArgs args)
        {
            if (IsAborting)
                throw new ExecutionAbortException(Properties.Language.ExecutionAborted);
            LeaveNode?.Invoke(this, args);
            if (_parent != null)
                _parent.FireLeaveNode(args);
        }
        ExecutionEnvironment _parent=null;
        Class _current;
        public int Level
        {
            get
            {
                int level = 1;
                ExecutionEnvironment p = this; ;
                while ((p = p.Parent) != null)
                    level++;
                return level;
            }
        }
        public ExecutionEnvironment Parent
        {
            get
            {
                return _parent;
            }
        }
        public Class Module
        {
            get
            {
                if (_current != null)
                    return _current;
                if (Parent != null)
                {
                    return Parent.Module;
                }
                return null;
            }
        }
        public void Step()
        {

        }
        public void Pause()
        {

        }
        public void Continue()
        {

        }
        public void Stop()
        {
            IsAborting = true;
        }
        public void ExecuteAsync(Class m)
        {
            new Thread(() =>
            {
                try
                {
                    Execute(m);
                }catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                }
            }).Start();
        }
        public Completion Execute(Class m)
        {
            IsAborting = false;
            IsCompleted = false;
            _current = m;
            if (_current == null)
                return null;
            Completion c = Completion.Void;
            ExecutionEnvironment baseEnv = this.GetBaseEnvironment();
            ExecutionEnvironment classEnv = new ExecutionEnvironment(baseEnv);
            ExecutionEnvironment instanceEnv = new ExecutionEnvironment(classEnv);
            foreach (var v in m.Variables)
            {
                instanceEnv.RegisterValue(v.Name, v.Value);
            }
            foreach (var func in _current.Functions)
            {
                if ("main".Equals(func.Name, StringComparison.OrdinalIgnoreCase))
                {
                    var parameter = func.Params;
                    ExecutionEnvironment functionEnv = new ExecutionEnvironment(instanceEnv);
                    ExecutionStarted?.Invoke(this, null);
                    foreach(var p in parameter)
                    {
                        functionEnv.RegisterValue(p.Name, null);
                    }
                    foreach(var block in m.BlockStatements) { 
                        foreach(var s in block.Body)
                        {
                            if(s is ExpressionStatement)
                            {
                                Expression exp = (s as ExpressionStatement).Expression;
                                if (exp is VariableDeclarationExpression)
                                    exp.Execute(instanceEnv);
                            }
                        }
                    }
                    try
                    {
                        IsCompleted = false;
                        c=func.Execute(functionEnv);
                        break;
                    }catch(Exception e)
                    {
                        IsCompleted = true;
                        Console.WriteLine(e.Message);
                        Console.WriteLine(e.StackTrace);
                        ExecutionAborted?.Invoke(this, new Completion(e.Message, CompletionType.Exception));
                        return Completion.Void;

                    }
                }
            }
            IsCompleted = true;
            if (c.Type == CompletionType.Value)
                ExecutionCompleted?.Invoke(this, c);
            else if (c.Type == CompletionType.Exception)
                ExecutionAborted?.Invoke(this, c);
            else
                ExecutionAborted?.Invoke(this, Completion.Exception(Properties.Language.UnknowException, null));
            return c;
        }

        public ExecutionEnvironment()
        {
            _parent = null;
        }
        public ExecutionEnvironment(ExecutionEnvironment parent)
        {
            _parent = parent;
        }
        public void Dump(TextWriter writer)
        {
            writer.WriteLine("Environment level " + Level);
            foreach (var key in currentVariables.Keys)
            {
                object value = currentVariables[key];
                writer.WriteLine("{0}={1} type={2}", key, value, value == null ? "Null" : value.GetType().ToString());
            }
            if (Parent != null)
            {
                Parent.Dump(writer);
            }
        }
        Dictionary<string, object> currentVariables = new Dictionary<string, object>();
        Dictionary<string, DelegateFunction> currentFunctions = new Dictionary<string, DelegateFunction>();
        Dictionary<string, ExecutionEnvironment> currentModules = new Dictionary<string, ExecutionEnvironment>();
        
        public DelegateFunction GetFunction(string name)
        {
            if (IsAborting)
                throw new ExecutionAbortException(Properties.Language.ExecutionAborted);
            if (currentFunctions.ContainsKey(name))
                return currentFunctions[name];
            if (_parent != null)
                return _parent.GetFunction(name);
            return null;
        }
        public void RegisterFunction(string name, DelegateFunction func)
        {
            if (IsAborting)
                throw new ExecutionAbortException(Properties.Language.ExecutionAborted);
            currentFunctions[name] = func;
        }
        public bool HasFunction(string name)
        {
            if (IsAborting)
                throw new ExecutionAbortException(Properties.Language.ExecutionAborted);
            if (currentVariables.ContainsKey(name))
                return true;
            if (_parent != null)
                return _parent.HasFunction(name);
            return false;
        }
        public bool HasValue(string variable)
        {
            if (IsAborting)
                throw new ExecutionAbortException(Properties.Language.ExecutionAborted);
            if (currentVariables.ContainsKey(variable))
                return true;
            if (_parent != null)
                return _parent.HasValue(variable);
            return false;
        }
        public void RegisterValue(string variable, object value)
        {
            if (IsAborting)
                throw new ExecutionAbortException(Properties.Language.ExecutionAborted);
            if (currentVariables.ContainsKey(variable))
                throw new Exception(Properties.Language.VariableDefinedExcepiton);
            currentVariables.Add(variable, value);
        }
        /// <summary>
        /// get function and variable value
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        public object GetValue(string variable)
        {
            if (IsAborting)
                throw new ExecutionAbortException(Properties.Language.ExecutionAborted);
            if (currentVariables.ContainsKey(variable))
            {
                return currentVariables[variable];
            }
            if (_parent != null)
                return _parent.GetValue(variable);
            throw new KeyNotFoundException();
        }
        public T GetValue<T>(string variable)
        {
            if (IsAborting)
                throw new ExecutionAbortException(Properties.Language.ExecutionAborted);
            object value = GetValue(variable);
            if (value == null)
            {
                return default(T);
            }
            return TypeConverters.GetValue<T>(value);

        }
        public void SetValue(string variable, object value)
        {
            if (IsAborting)
                throw new ExecutionAbortException(Properties.Language.ExecutionAborted);
            if (currentVariables.ContainsKey(variable))
            {
                currentVariables[variable] = value;
                return;
            }
            else if (_parent != null)
            {
                _parent.SetValue(variable, value);
                return;
            }
            else
                throw new KeyNotFoundException();
        }
    }
}
