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
        public string ReturnType
        {
            get { return "void"; }
        }
        public Completion Execute(ExecutionEnvironment enviroment)
        {
            return Completion.Void;
        }

        public Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "设置旋转模式"));
                desc.Add(new SelectionItemDescriptor(this, "RotationMode", new object[] { "左右翻转", "不翻转", "任意" },
                    new object[] { CharacterRotationMode.LeftRight, CharacterRotationMode.None, CharacterRotationMode.Any}));
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
        }//execution  

        public ExecutionEnvironment StartCall(ExecutionEnvironment e)
        {
            return e;
        }
        public Completion EndCall(ExecutionEnvironment _env)
        {
            Sprite sp = (_env.This as Instance).Class as Sprite;
            if (sp.RotationMode != RotationMode)
            {
                ((_env.This as Instance).Class as Sprite).RotationMode = RotationMode;
                App.Current.Dispatcher.InvokeAsync(new Action(() =>
                {
                    (_env.GetValue("$$Player") as ScriptPlayer).DrawScript();
                }));
            }

            return Completion.Void;
        }

        public bool PopStack(out object execution, out ExecutionCallback callback, ExecutionEnvironment e)
        {
            execution = null;
            callback = Callback;
            return false;
        }
        Nullable<DateTime> Callback(object value, object exception, ExecutionEnvironment e)
        {
            return null;
        }
        public bool HandleException(object exception)
        {
            throw new NotImplementedException();
        }
    }
}
