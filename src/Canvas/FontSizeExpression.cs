using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class FontSizeExpression : Expression, IAssignment
    {
        public FontSizeExpression()
        {

        }
        public override string ReturnType => "number";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "fontSize"));
                return desc;
            }
        }

        public override string Type => "FontSizeExpression";

        public Completion Assign(ExecutionEnvironment environment, object value)
        {
            if (!TypeConverters.IsNumber(value))
                return Completion.Exception(Properties.Language.ValueNotNumberException, this);
            DrawWindow wnd = CanvasEnvironment.GetCanvas(environment);
            wnd.TextSize = TypeConverters.GetValue<double>(value);
            return new Completion(value);
        }

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            DrawWindow wnd = CanvasEnvironment.GetCanvas(enviroment);

            object v = wnd.TextSize;
            return new Completion(v);
        }
    }
}
