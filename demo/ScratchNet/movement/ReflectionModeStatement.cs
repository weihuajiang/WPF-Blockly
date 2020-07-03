using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class ReflectionStatement : Statement
    {
        public ReflectionStatement()
        {
        }
        public override string ReturnType
        {
            get { return "void"; }
        }
        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if(enviroment.HasValue("$$ReflectionOnTouchSide&&"))
            enviroment.SetValue("$$ReflectionOnTouchSide&&", true);
            else
                enviroment.RegisterValue("$$ReflectionOnTouchSide&&", true);

            return Completion.Void;
        }

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, Localize.GetString("m_Wall")));
                return desc;
            }
        }
        public override string Type
        {
            get
            {
                return "ReflectionStatement";
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
