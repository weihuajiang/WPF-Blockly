using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class SpeachBase
    {
        static string Name = ">>>>>Speech<<<<<<";
        public static SpeechSynthesizer GetSpeech(ExecutionEnvironment environment)
        {
            SpeechSynthesizer Speach = null;
            if (environment.HasValue(Name))
                Speach = environment.GetValue(Name) as SpeechSynthesizer;
            else
            {
                Speach = new SpeechSynthesizer();
                environment.GetBaseEnvironment().RegisterValue(Name, Speach);
            }
            return Speach;
        }
    }
}
