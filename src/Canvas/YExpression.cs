using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class YExpression : Expression, IAssignment
    {
        public YExpression()
        {

        }
        public override string ReturnType => "number";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "y"));
                return desc;
            }
        }

        public override string Type => "YExpression";

        public Completion Assign(ExecutionEnvironment environment, object value)
        {
            if (!TypeConverters.IsNumber(value))
                return Completion.Exception(Properties.Language.ValueNotNumberException, this);
            DrawWindow wnd = CanvasEnvironment.GetCanvas(environment);
            wnd.Y = TypeConverters.GetValue<double>(value);
            wnd.Goto(wnd.X, wnd.Y);
            return new Completion(value);
        }

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            DrawWindow wnd = CanvasEnvironment.GetCanvas(enviroment);

            object v = wnd.Y;
            return new Completion(v);
        }
    }
}
