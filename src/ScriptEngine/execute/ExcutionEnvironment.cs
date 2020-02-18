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
        public event ExecutionEnvironmentEventHandler<object> ExecutionCompleted;
        public event ExecutionEnvironmentEventHandler<object> ExecutionStarted;
        public event ExecutionEnvironmentEventHandler<Completion> ExecutionAborted;
        public bool IsCompleted { get; internal set; } = false;

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
        public void ExecuteAsync(Class m)
        {
            new Thread(() =>
            {
                Execute(m);
            }).Start();
        }
        public Completion Execute(Class m)
        {
            _current = m;
            if (_current == null)
                return null;
            Completion c = Completion.Void;
            foreach (var func in _current.Functions)
            {
                if ("main".Equals(func.Format, StringComparison.OrdinalIgnoreCase))
                {
                    foreach(var v in _current.Variables)
                    {
                        RegisterValue(v.Name, v.Value);
                    }
                    var parameter = func.Params;
                    ExecutionEnvironment current = new ExecutionEnvironment(this);
                    ExecutionStarted?.Invoke(this, null);
                    foreach(var p in parameter)
                    {
                        current.RegisterValue(p.Name, null);
                    }
                    try
                    {
                        IsCompleted = false;
                        c=func.Execute(current);
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
                ExecutionAborted?.Invoke(this, Completion.Exception("Unknown exception", null));
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
        public void Dump(StreamWriter writer)
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
            if (currentFunctions.ContainsKey(name))
                return currentFunctions[name];
            if (_parent != null)
                return _parent.GetFunction(name);
            return null;
        }
        public void RegisterFunction(string name, DelegateFunction func)
        {
            currentFunctions[name] = func;
        }
        public bool HasFunction(string name)
        {
            if (currentVariables.ContainsKey(name))
                return true;
            if (_parent != null)
                return _parent.HasFunction(name);
            return false;
        }
        public bool HasValue(string variable)
        {
            if (currentVariables.ContainsKey(variable))
                return true;
            if (_parent != null)
                return _parent.HasValue(variable);
            return false;
        }
        public void RegisterValue(string variable, object value)
        {
            currentVariables[variable]=value;
        }
        /// <summary>
        /// get function and variable value
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        public object GetValue(string variable)
        {
            if (currentVariables.ContainsKey(variable))
            {
                return currentVariables[variable];
            }
            if (_parent != null)
                return _parent.GetValue(variable);
            return null;
        }
        public T GetValue<T>(string variable)
        {
            object value = GetValue(variable);
            if (value == null)
            {
                return default(T);
            }
            return TypeConverters.GetValue<T>(value);

        }
        public void SetValue(string variable, object value)
        {
            if (currentVariables.ContainsKey(variable))
            {
                currentVariables[variable] = value;
                return;
            }
            if (_parent != null)
            {
                _parent.SetValue(variable, value);
                return;
            }
            currentVariables[variable]=value;
        }
    }
}
