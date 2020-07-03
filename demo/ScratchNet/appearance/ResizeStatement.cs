using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class ResizeStatement : Statement
    {
        public ResizeStatement()
        {
            Size = new Literal() { Raw = "90" };
        }
        public Expression Size { get; set; }
        public override string ReturnType
        {
            get { return "void"; }
        }
        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Size == null)
                return Completion.Void;
            var c = Size.Execute(enviroment);
            if (c.Type != CompletionType.Value)
                return c;
            int s = 0;
            try
            {
                s = TypeConverters.GetValue<int>(c.ReturnValue);
            }catch(Exception e)
            {
                return Completion.Exception("Wrong number format", Size);
            }
            Sprite sp = enviroment.GetValue("$$INSTANCE$$") as Sprite;
            sp.Size = s;
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
                desc.Add(new TextItemDescriptor(this, Localize.GetString("cs_SetSizeTo")));
                desc.Add(new ExpressionDescriptor(this, "Size", "number"));
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
