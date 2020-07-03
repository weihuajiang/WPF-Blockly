using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class XPositionExpression : Expression
    {
        public override string ReturnType
        {
            get { return "number"; }
        }

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            double result = (enviroment.GetValue("$$INSTANCE$$") as Sprite).X;
            return new Completion(result);
        }

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "X"));
                return desc;
            }
        }

        public override string Type
        {
            get { return "XPositionExpression"; }
        }
        public bool IsClosing
        {
            get { return false; }
        }
    }
}
