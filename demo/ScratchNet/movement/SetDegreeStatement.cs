using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class SetDegreeStatement : Statement
    {
        public SetDegreeStatement()
        {
            Degree = new Literal() { Raw = "0" };
        }
        public Expression Degree { get; set; }
        public override string ReturnType
        {
            get { return "void"; }
        }
        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Degree == null)
                return Completion.Void;
            double degree = 0;
            try
            {
                var c = Degree.Execute(enviroment);
                if (c.Type != CompletionType.Value)
                    return c;
                degree = TypeConverters.GetValue<double>(c.ReturnValue);
            }
            catch
            {
                return Completion.Exception("Wrong number format", Degree);
            }

            Sprite sp = enviroment.GetValue("$$INSTANCE$$") as Sprite;
            if (sp.Direction != (int)degree)
            {
                sp.Direction = (int)degree;
                App.Current.Dispatcher.InvokeAsync(new Action(() =>
                {
                    (enviroment.GetValue("$$Player") as ScriptPlayer).DrawScript();
                }));
            }
            return Completion.Void;
        }

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, Localize.GetString("cs_SetDirection")));
                desc.Add(new ExpressionDescriptor(this, "Degree", "number"));
                desc.Add(new TextItemDescriptor(this, Localize.GetString("cs_Degree")));
                return desc;
            }
        }
        public override string Type
        {
            get
            {
                return "RotationStatement";
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
