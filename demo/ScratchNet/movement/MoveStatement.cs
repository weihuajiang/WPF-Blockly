using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class MoveStatement : Statement
    {
        public MoveStatement()
        {
            Step = new Literal() { Raw = "5" };
        }
        public Expression Step { get; set; }
        public string ReturnType
        {
            get { return "void"; }
        }
        public Completion Execute(ExecutionEnvironment enviroment)
        {
            if (Step == null)
                return Completion.Void;
            double degree = 0;
            try
            {
                var c = Step.Execute(enviroment);
                if (c.Type != CompletionType.Value)
                    return c;
                degree = TypeConverters.GetValue<double>(c.ReturnValue);
            }
            catch
            {
                return Completion.Exception("Wrong number format", Step);
            }
            bool reflection = false;
            if (enviroment.HasValue("$$ReflectionOnTouchSide&&") && enviroment.GetValue<bool>("$$ReflectionOnTouchSide&&"))
                reflection = true;
            Sprite sp = enviroment.GetValue("$$INSTANCE$$") as Sprite;
            int x = sp.X + (int)(Math.Cos(sp.Direction * Math.PI / 180) * degree);
            int y = sp.Y + (int)(Math.Sin(sp.Direction * Math.PI / 180) * degree);
            int direction = sp.Direction;
            if (reflection)
            {
                while (x < 0)
                {
                    x = -x;
                    direction = 180 - direction;
                }
                while (x > CurrentEnviroment.ScreenWidth)
                {
                    x = CurrentEnviroment.ScreenWidth * 2 - x;
                    direction = 180 - direction;
                }
                while (y < 0)
                {
                    y = -y;
                    direction = -direction;
                }
                while (y > CurrentEnviroment.ScreenHeight)
                {
                    y = CurrentEnviroment.ScreenHeight * 2 - y;
                    direction = -direction;
                }
            }
            sp.X = x;
            sp.Y = y;
            sp.Direction = direction;
            App.Current.Dispatcher.InvokeAsync(new Action(() =>
            {
                (enviroment.GetValue("$$Player") as ScriptPlayer).DrawScript();
            }));
            return Completion.Void;
        }

        public Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, Localize.GetString("m_Move")));
                desc.Add(new ExpressionDescriptor(this, "Step", "number"));
                desc.Add(new TextItemDescriptor(this, Localize.GetString("m_Step")));
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
