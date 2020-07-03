using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class SetImageStatement : Statement
    {
        public SetImageStatement()
        {
        }
        public Expression ImageIndex { get; set; }
        public override string ReturnType
        {
            get { return "void"; }
        }
        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (ImageIndex == null)
                return Completion.Void;
            var c = ImageIndex.Execute(enviroment);
            if (c.Type != CompletionType.Value)
                return c;
            int currentIndex = 0;
            Sprite sp = enviroment.GetValue("$$INSTANCE$$") as Sprite;
            if (TypeConverters.IsNumber(c.ReturnValue))
            {
                try
                {
                    currentIndex = TypeConverters.GetValue<int>(c.ReturnValue);
                }
                catch
                {
                    return Completion.Exception("Wrong number format", ImageIndex);
                }
            }
            else
            {
                string name = c.ReturnValue + "";
                int i = 0;
                foreach (Resource r in sp.Images)
                {
                    if (r.DisplayName == name)
                        currentIndex = i;
                    i++;
                }
            }
            if (currentIndex < 0)
                currentIndex = 0;
            if (currentIndex >= sp.Images.Count)
                currentIndex = 0;
            sp.CurrentImage = currentIndex;
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
                desc.Add(new TextItemDescriptor(this, Localize.GetString("cs_ChangePicture")));
                desc.Add(new SelectionExpressionItemDescriptor(this, "ImageIndex", CurrentEnviroment.CurrentSpriteImages,
                    new ExpressionDescriptor(this, "ImageIndex", "number")));
                return desc;
            }
        }
        public override string Type
        {
            get
            {
                return "ResizeStatement";
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
