using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class HideStatement : Statement
    {
        public string ReturnType
        {
            get { return "void"; }
        }
        public Completion Execute(ExecutionEnvironment enviroment)
        {
            Sprite sp = enviroment.GetValue("$$INSTANCE$$") as Sprite;
            if (sp.Visible)
            {
                sp.Visible = false; ;
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
                desc.Add(new TextItemDescriptor(this, Localize.GetString("a_Hide")));
                return desc;
            }
        }
        public string Type
        {
            get
            {
                return "ShowStatement";
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
