using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class XExpression : Expression, IAssignment
    {
        public XExpression()
        {

        }
        public override string ReturnType => "number";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "x"));
                return desc;
            }
        }

        public override string Type => "XExpression";

        public Completion Assign(ExecutionEnvironment environment, object value)
        {
            if (!TypeConverters.IsNumber(value))
                return Completion.Exception(Properties.Language.ValueNotNumberException, this);
            DrawWindow wnd = CanvasEnvironment.GetCanvas(environment);
            wnd.X=TypeConverters.GetValue<double>(value);
            wnd.Goto(wnd.X, wnd.Y);
            return new Completion(value);
        }

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            DrawWindow wnd = CanvasEnvironment.GetCanvas(enviroment);

            object v = wnd.X;
            return new Completion(v);
        }
    }
}
