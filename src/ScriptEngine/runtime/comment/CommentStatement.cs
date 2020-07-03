using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class CommentStatement : Statement
    {
        public override string ReturnType => "any";

        public override Descriptor Descriptor
        {
            get
            {
                if (AllowMultiLine)
                {
                    Descriptor d = new Descriptor();
                    d.Add(new TextItemDescriptor(this, "/* ") { VerticalAlignment = VerticalAlignmentEnum.Top } );
                    d.Add(new MultiLineStringInputDesciptor(this, "Comment"));
                    d.Add(new TextItemDescriptor(this, " */") { VerticalAlignment = VerticalAlignmentEnum.Bottom });
                    return d;
                }
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "// "));
                desc.Add(new StringInputDesciptor(this, "Comment"));
                return desc;
            }
        }

        public override string Type => "any";
        public String Comment { get; set; } = "";
        public bool AllowMultiLine { get; set; } = false;
        public override BlockDescriptor BlockDescriptor
        {
            get { 
                return null;
            }
        }

        public override bool IsClosing => false;

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            return Completion.Void;
        }
    }
}
