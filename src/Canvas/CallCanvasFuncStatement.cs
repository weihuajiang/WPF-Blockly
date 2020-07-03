using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class CallCanvasFuncStatement : Statement
    {
        public CallCanvasFuncStatement()
        {

        }
        public CallCanvasFuncStatement(string func, string display)
        {
            Func = func;
            Display = display;
        }
        public string Display { get; set; } = "";
        public string Func { get; set; } = "";

        public override string ReturnType => "void";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, Display+";"));
                return desc;
            }
        }

        public override string Type => "LineStatement";

        public override BlockDescriptor BlockDescriptor => null;

        public override bool IsClosing => false;

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            DrawWindow wnd = CanvasEnvironment.GetCanvas(enviroment);
            MethodInfo m = wnd.GetType().GetMethod(Func);
            if (m == null)
                return Completion.Exception("No " + Func + " function in canvas", this);
            m.Invoke(wnd,null);
            return Completion.Void;
        }
    }
}
