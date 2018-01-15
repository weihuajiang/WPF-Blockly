using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class ExecutionEnvironment
    {
        ExecutionEnvironment _parent;
        Instance _this;
        public Instance This
        {
            get
            {
                if (_this != null)
                    return _this;
                if (_parent != null)
                    return _parent.This;
                return null;
            }
        }
        public int Level
        {
            get
            {
                int level = 1;
                ExecutionEnvironment p = this;;
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
        public IExecutionEngine Engine { get; internal set; }
        public ExecutionEnvironment(IExecutionEngine engine)
        {
            Engine = engine;
            currentScope = new Dictionary<string, object>();
        }
        public ExecutionEnvironment(ExecutionEnvironment parent)
        {
            _parent = parent;
            Engine = parent.Engine;
            currentScope = new Dictionary<string, object>();
        }
        public ExecutionEnvironment(ExecutionEnvironment parent, Instance instance)
        {
            _parent = parent;
            Engine = parent.Engine;
            currentScope = instance.States;
            _this = instance;
        }
        Dictionary<string, object> currentScope;
        Dictionary<string, object> state = new Dictionary<string, object>();
        public void SetState(string name, object v)
        {
            state[name] = v;
        }
        public object GetState(string name)
        {
            return state[name];
        }
        public void ClearState(string name)
        {
            if (state.ContainsKey(name))
                state.Remove(name);
        }
        public bool Has(string variable)
        {
            if (currentScope.ContainsKey(variable))
                return true;
            if (_parent!=null && _parent.Has(variable))
                return true;
            return false;
        }
        public void RegisterValue(string variable, object value)
        {
            currentScope.Add(variable, value);
        }
        public object GetValue(string variable)
        {
            if (currentScope.ContainsKey(variable))
            {
                return currentScope[variable];
            }
            if (_parent != null)
                return _parent.GetValue(variable);
            return null;
        }
        public void SetValue(string variable, object value)
        {
            if (currentScope.ContainsKey(variable))
            {
                currentScope[variable] = value;
                return;
            }
            if (_parent != null && _parent.Has(variable))
            {
                _parent.SetValue(variable, value);
                return;
            }
            currentScope.Add(variable, value);
        }
    }
}
