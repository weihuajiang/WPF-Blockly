using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class XPositionExpression : Expression
    {
        public string ReturnType
        {
            get { return "number"; }
        }

        public Completion Execute(ExecutionEnvironment enviroment)
        {
            double result = (enviroment.GetValue("$$INSTANCE$$") as Sprite).X;
            return new Completion(result);
        }

        public Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "X"));
                return desc;
            }
        }

        public string Type
        {
            get { return "XPositionExpression"; }
        }
        public bool IsClosing
        {
            get { return false; }
        }
    }
}
