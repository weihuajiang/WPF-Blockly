using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class SetPositionXStatement : Statement
    {
        public SetPositionXStatement()
        {
            X = new Literal() { Raw = "5" };
        }
        public Expression X { get; set; }
        public override string ReturnType
        {
            get { return "void"; }
        }
        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (X == null)
                return Completion.Void;
            double x = 0;
            try
            {
                var c = X.Execute(enviroment);
                if (c.Type != CompletionType.Value)
                    return c;
                x = TypeConverters.GetValue<double>(c.ReturnValue);
            }
            catch
            {
                return Completion.Exception("Wrong number format", X);
            }
            Sprite sp = enviroment.GetValue("$$INSTANCE$$") as Sprite;
            sp.X = (int)x;
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
                desc.Add(new TextItemDescriptor(this, Localize.GetString("cs_X_CoordinatesSetTo")));
                desc.Add(new ExpressionDescriptor(this, "X", "number"));
                return desc;
            }
        }
        public override string Type
        {
            get
            {
                return "SetPositionStatement";
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
