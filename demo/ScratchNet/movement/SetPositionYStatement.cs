using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class SetPositionYStatement : Statement
    {
        public SetPositionYStatement()
        {
            Y = new Literal() { Raw = "5" };
        }
        public Expression Y { get; set; }
        public override string ReturnType
        {
            get { return "void"; }
        }
        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Y == null)
                return Completion.Void;
            double y = 0;
            try
            {
                var c = Y.Execute(enviroment);
                if (c.Type != CompletionType.Value)
                    return c;
                y = TypeConverters.GetValue<double>(c.ReturnValue);
            }
            catch
            {
                return Completion.Exception("Wrong number format", Y);
            }
            Sprite sp = enviroment.GetValue("$$INSTANCE$$") as Sprite;
            sp.Y = (int)y;
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
                desc.Add(new TextItemDescriptor(this, Localize.GetString("cs_Y_CoordinatesSetTo")));
                desc.Add(new ExpressionDescriptor(this, "Y", "number"));
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
