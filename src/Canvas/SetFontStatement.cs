using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ScratchNet
{
    public class SetFontStatement : Statement
    {
        public string FontFamily { get; set; }
        public FontStyle Style { get; set; } = FontStyles.Normal;
        public FontWeight Weight { get; set; } = FontWeights.Normal;

        public override string ReturnType => "void";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "setFont("));
                List<string> fonts = new List<string>();
                List<string> names = new List<string>();
                foreach(var f in Fonts.SystemFontFamilies)
                {
                    fonts.Add(f.Source);
                    names.Add(f.Source);
                }
                desc.Add(new SelectionItemDescriptor(this, "FontFamily", names, fonts));
                desc.Add(new TextItemDescriptor(this, ", "));

                List<FontStyle> styles = new List<FontStyle>();
                List<string> styleNames = new List<string>();
                styles.Add(FontStyles.Normal);
                styles.Add(FontStyles.Italic);
                styleNames.Add("normal");
                styleNames.Add("italc");
                desc.Add(new SelectionItemDescriptor(this, "Style", styleNames, styles));
                desc.Add(new TextItemDescriptor(this, ", "));

                List<FontWeight> weights = new List<FontWeight>();
                List<string> wnames = new List<string>();
                weights.Add(FontWeights.Normal);
                weights.Add(FontWeights.Bold);
                wnames.Add("normal");
                wnames.Add("bold");
                desc.Add(new SelectionItemDescriptor(this, "Weight", wnames, weights));

                desc.Add(new TextItemDescriptor(this, ");"));
                return desc;
            }
        }

        public override string Type => "SetFontStatement";

        public override BlockDescriptor BlockDescriptor => null;

        public override bool IsClosing => false;

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (FontFamily == null)
                return Completion.Exception(Properties.Language.NullException, this);
            Typeface face = null;
            foreach (var f in Fonts.SystemTypefaces)
            {
                if (f.FontFamily.Equals(FontFamily) && f.Style.Equals(Style) && f.Weight.Equals(Weight))
                {
                    face = f;
                    break;
                }
            }
            if (face == null)
            {
                foreach (var f in Fonts.SystemTypefaces)
                {
                    if (f.FontFamily.Equals(FontFamily) && f.Style.Equals(Style))
                    {
                        face = f;
                        break;
                    }
                }
            }
            if (face == null)
                face = new Typeface(FontFamily);
            DrawWindow wnd = CanvasEnvironment.GetCanvas(enviroment);
            wnd.Font = face;
            return Completion.Void;
        }
    }
}
