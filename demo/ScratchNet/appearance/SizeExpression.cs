using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class SizeExpression : Expression
    {
        public override string ReturnType
        {
            get { return "number"; }
        }

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            Sprite sp = enviroment.GetValue("$$INSTANCE$$") as Sprite;
            return new Completion(sp.Size);
        }

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "Size"));
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
