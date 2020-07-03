using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ScratchNet
{
    public class ClearStatement : ConsoleBaseStatement
    {
        public override string ReturnType
        {
            get { return "void"; }
        }
        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            ConsoleWindow wnd = GetConsole(enviroment);
            wnd.Clear();
            return Completion.Void;
        }
        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "clear();"));
                return desc;
            }
        }
        public override string Type
        {
            get
            {
                return "ClearStatement";
            }
        }
        public override BlockDescriptor BlockDescriptor => null;
        public override bool IsClosing => false;
    }
}
