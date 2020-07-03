using ScratchNet.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class SpeechLibrary : Library
    {
        public SpeechLibrary()
        {
            Name = "Speech";
            Title = Language.Title;
            DefaultColor = "#ea9bfa";
            Description = Language.Description;
            CommandGroup speak = new CommandGroup(Language.SpeakCategory, Language.SpeakCategoryDesc);
            speak.Add(new Command("speak", Language.SpeakDescription, true, new SpeekStatement()));
            speak.Add(new Command("speakkAsync", Language.SpeakAsyncDescription, true, new SpeekAsyncStatement()));
            Add(speak);
            CommandGroup setting = new CommandGroup(Language.SettingCategory, Language.SettingCategoryDesc);
            setting.Add(new Command("setlect", Language.SelectDescription, true, new SelectVoiceStatement()));
            setting.Add(new Command("volume", Language.VolumeDescription, true, new SpeakVolumeExpression()));
            setting.Add(new Command("rate", Language.RateDescription, true, new SpeakRateExpression()));
            Add(setting);
        }
    }
}
