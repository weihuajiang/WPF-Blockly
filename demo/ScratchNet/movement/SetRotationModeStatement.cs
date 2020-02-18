using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class SetRotationModeStatement : Statement
    {
        public CharacterRotationMode RotationMode { get; set; }
        public SetRotationModeStatement()
        {
            RotationMode = CharacterRotationMode.None;
        }
        public string ReturnType
        {
            get { return "void"; }
        }
        public Completion Execute(ExecutionEnvironment enviroment)
        {
            Sprite sp = enviroment.GetValue("$$INSTANCE$$") as Sprite;
            if (sp.RotationMode != RotationMode)
            {
                sp.RotationMode = RotationMode;
                App.Current.Dispatcher.InvokeAsync(new Action(() =>
                {
                    (enviroment.GetValue("$$Player") as ScriptPlayer).DrawScript();
                }));
            }

            return Completion.Void;
        }

        public Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, Localize.GetString("cs_SetRotationMode")));
                desc.Add(new SelectionItemDescriptor(this, "RotationMode", new object[] { Localize.GetString("cs_LRFlip"), Localize.GetString("cs_NoFlip"), Localize.GetString("cs_Arbitrary") },
                    new object[] { CharacterRotationMode.LeftRight, CharacterRotationMode.None, CharacterRotationMode.Any}));
                return desc;
            }
        }
        public string Type
        {
            get
            {
                return "MoveStatement";
            }
        }


        public BlockDescriptor BlockDescriptor
        {
            get { return null; }
        }


        public bool IsClosing
        {
            get { return false; }
        }
    }
}
