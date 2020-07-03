using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class ReadLineExpression : ConsoleBaseExpression
    {
        public ReadLineExpression()
        {
        }
        public override string ReturnType
        {
            get { return "string"; }
        }
        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            ConsoleWindow wnd = GetConsole(enviroment);
            return new Completion(wnd.ReadLine());
        }
        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "readln()"));
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
