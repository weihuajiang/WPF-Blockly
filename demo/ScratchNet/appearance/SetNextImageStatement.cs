using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class SetNextImageStatement : Statement
    {
        public override string ReturnType
        {
            get { return "void"; }
        }
        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            Sprite sp = enviroment.GetValue("$$INSTANCE$$") as Sprite;
            int current = sp.CurrentImage;
            current++;
            if (current == sp.Images.Count)
                current = 0;
            sp.CurrentImage = current;
            App.Current.Dispatcher.InvokeAsync(new Action(() =>
            {
                (enviroment.GetValue("$$Player") as ScriptPlayer).DrawScript();
            }));
            return Completion.Void;
        }

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, Localize.GetString("cs_ShowNextImg")));
                return desc;
            }
        }
        public override string Type
        {
            get
            {
                return "SetNextImageStatement";
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
