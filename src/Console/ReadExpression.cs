using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class ReadExpression : ConsoleBaseExpression
    {
        public ReadExpression()
        {
        }
        public override string ReturnType
        {
            get { return "string"; }
        }
        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            ConsoleWindow wnd = GetConsole(enviroment);
            return new Completion(wnd.Read());
        }
        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "read()"));
                return desc;
            }
        }
        public override string Type
        {
            get
            {
                return "ReadLineStatement";
            }
        }

    }
}
