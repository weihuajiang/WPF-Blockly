using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ScratchNet
{
    public class ColorExpression : Expression
    {
        public string DisplayText { get; set; }
        public Color Color { get; set; } = Colors.Green;

        public override string ReturnType => "color";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, DisplayText));
                return desc;
            }
        }

        public override string Type => "ColorExpression";

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            return new Completion(Color);
        }
    }
}
