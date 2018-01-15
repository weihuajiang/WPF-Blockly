using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class ExecutionEnvironment
    {
        ExecutionEnvironment _parent;
        public ExecutionEnvironment Parent
        {
            get
            {
                return _parent;
            }
        }
        public ExecutionEnvironment(ExecutionEnvironment parent)
        {
            _parent = parent;
        }
        Dictionary<string, object> currentScope = new Dictionary<string, object>();
        public bool Has(string variable)
        {
            if (currentScope.ContainsKey(variable))
                return true;
            if (_parent!=null && _parent.Has(variable))
                return true;
            return false;
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
