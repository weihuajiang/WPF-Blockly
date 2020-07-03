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
        public override string ReturnType
        {
            get { return "void"; }
        }
        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            Sprite sp = enviroment.GetValue("$$INSTANCE$$") as Sprite;
            if (sp.RotationMode != RotationMode)
            {
                sp.RotationMode = RotationMode;
            }

            return Completion.Void;
        }

        public override Descriptor Descriptor
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
        public override string Type
        {
            get
            {
                return "MoveStatement";
            }
        }


        public override BlockDescriptor BlockDescriptor
        {
            get { return null; }
        }


        public override bool IsClosing
        {
            get { return false; }
        }
    }
}
