using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class ScriptStep
    {
        public ScriptStep(Object step, bool isEditable = false, string tooltip=null)
        {
            StepObject = step;
            IsColorEditable = isEditable;
            Tooltip = tooltip;
        }
        public object StepObject { get; set; }
        public bool IsColorEditable { get; set; } = false;
        public string Tooltip { get; set; }
    }
    public class ScriptStepGroup
    {
        public string Name { get; set; }
        public List<object> Types { get; set; }
        public void RemoveAllType(Type t)
        {
            bool cont=true;
            while (cont)
            {
                foreach (object obj in Types)
                {
                    if (obj.GetType().IsAssignableFrom(t))
                    {
                        Types.Remove(obj);
                        break;
                    }
                }
                cont = false;
                foreach (object obj in Types)
                {
                    if (obj.GetType().IsAssignableFrom(t))
                    {
                        cont = true;
                    }
                }
                
            }
        }
    }
}
