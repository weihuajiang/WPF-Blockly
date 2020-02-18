using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class DegreeExpression: Expression
    {
        public string ReturnType
        {
            get { return "number"; }
        }

        public Completion Execute(ExecutionEnvironment enviroment)
        {
            Sprite sp = enviroment.GetValue("$$INSTANCE$$") as Sprite;
            return new Completion(sp.Direction);
        }

        public Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "Angle"));
                return desc;
            }
        }

        public string Type
        {
            get { return "DegreeExpression"; }
        }
        public bool IsClosing
        {
            get { return false; }
        }
    }
}
