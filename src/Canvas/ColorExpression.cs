using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ScratchNet
{
    public class LineColorExpression : Expression, IAssignment
    {
        public LineColorExpression()
        {

        }
        public override string ReturnType => "color";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "color"));
                return desc;
            }
        }

        public override string Type => "DrawColorExpression";

        public Completion Assign(ExecutionEnvironment environment, object value)
        {
            if (!(value is Color))
                return Completion.Exception(Properties.Language.NotColorException, this);
            DrawWindow wnd = CanvasEnvironment.GetCanvas(environment);
            wnd.Color = (Color)value;
            return new Completion(value);
        }

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            DrawWindow wnd = CanvasEnvironment.GetCanvas(enviroment);

            object v = wnd.Color;
            return new Completion(v);
        }
    }
}
