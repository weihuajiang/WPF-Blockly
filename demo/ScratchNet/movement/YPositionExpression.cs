using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{

    public class YPositionExpression : Expression
    {
        public string ReturnType
        {
            get { return "number"; }
        }

        public Completion Execute(ExecutionEnvironment enviroment)
        {
            double result = (enviroment.GetValue("$$INSTANCE$$") as Sprite).Y;
            return new Completion(result); ;
        }

        public Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "Y"));
                return desc;
            }
        }

        public string Type
        {
            get { return "YPositionExpression"; }
        }
        public bool IsClosing
        {
            get { return false; }
        }
    }
}
