using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class SelectVoiceStatement : Statement
    {
        public string Name { get; set; }
        public override string ReturnType => "void";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "selectVoice("));
                List<string> voice = new List<string>();
                foreach (var v in new SpeechSynthesizer().GetInstalledVoices())
                    voice.Add(v.VoiceInfo.Name);
                desc.Add(new SelectionItemDescriptor(this, "Name", voice, voice));
                desc.Add(new TextItemDescriptor(this, ");"));
                return desc;
            }
        }

        public override string Type => "SpeekStatement";

        public override BlockDescriptor BlockDescriptor => null;

        public override bool IsClosing => false;

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (!string.IsNullOrEmpty(Name))
                SpeachBase.GetSpeech(enviroment).SelectVoice(Name);
            return Completion.Void;
        }
    }
}
