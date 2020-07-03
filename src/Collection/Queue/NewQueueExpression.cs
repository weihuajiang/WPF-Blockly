using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class NewQueueExpression : Expression
    {
        public override string ReturnType => "queue";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "new", true));
                desc.Add(new TextItemDescriptor(this, " Queue()"));
                return desc;
            }
        }

        public override string Type => "NewQueueExpression";

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            return new Completion(new Queue<object>());
        }
    }
}
