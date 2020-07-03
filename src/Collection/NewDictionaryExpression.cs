using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class NewDictionaryExpression : Expression
    {
        public override string ReturnType => "dictionary";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "new", true));
                desc.Add(new TextItemDescriptor(this, " Dictionary()"));
                return desc;
            }
        }

        public override string Type => "NewDictionaryExpression";

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            return new Completion(new Dictionary<object, object>());
        }
    }
}
