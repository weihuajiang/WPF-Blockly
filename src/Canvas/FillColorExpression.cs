using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ScratchNet
{
    public class FillColorExpression: Expression, IAssignment
    {
        public FillColorExpression()
        {

        }
        public override string ReturnType => "color";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "fill"));
                return desc;
            }
        }

        public override string Type => "FillColorExpression";

        public Completion Assign(ExecutionEnvironment environment, object value)
        {
            if (!(value is Color))
                return Completion.Exception(Properties.Language.NotColorException, this);
            DrawWindow wnd = CanvasEnvironment.GetCanvas(environment);
            wnd.Fill = (Color)value;
            return new Completion(value);
        }

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            DrawWindow wnd = CanvasEnvironment.GetCanvas(enviroment);

            object v = wnd.Fill;
            return new Completion(v);
        }
    }
}
