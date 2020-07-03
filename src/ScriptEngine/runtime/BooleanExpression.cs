using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class TrueExpression : Expression
    {
        public override string ReturnType => "boolean";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "true", true));
                return desc;
            }
        }

        public override string Type => "TrueExpression";

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            return new Completion(true);
        }
    }
    public class FalseExpression : Expression
    {
        public override string ReturnType => "boolean";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "false", true));
                return desc;
            }
        }

        public override string Type => "FalseExpression";

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            return new Completion(false);
        }
    }
}
