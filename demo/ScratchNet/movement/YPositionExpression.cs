using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{

    public class YPositionExpression : Expression
    {
        public override string ReturnType
        {
            get { return "number"; }
        }

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            double result = (enviroment.GetValue("$$INSTANCE$$") as Sprite).Y;
            return new Completion(result); ;
        }

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "Y"));
                return desc;
            }
        }

        public override string Type
        {
            get { return "YPositionExpression"; }
        }
        public bool IsClosing
        {
            get { return false; }
        }
    }
}
