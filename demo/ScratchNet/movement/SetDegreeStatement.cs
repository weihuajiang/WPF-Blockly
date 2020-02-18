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
        public string ReturnType
        {
            get { return "void"; }
        }
        public Completion Execute(ExecutionEnvironment enviroment)
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

        public Descriptor Descriptor
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
        public string Type
        {
            get
            {
                return "RotationStatement";
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
