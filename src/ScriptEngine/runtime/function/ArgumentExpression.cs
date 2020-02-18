using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class ArgumentExpression: Expression
    {
        public String Variable { get; set; }
        public string VarType { get; set; }
        public string ReturnType
        {
            get { return VarType; }
        }

        public Completion Execute(ExecutionEnvironment enviroment)
        {
            return new Completion(enviroment.GetValue(Variable));
        }

        public Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, Variable));
                return desc;
            }
        }

        public string Type
        {
            get { return "ArgumentExpression"; }
        }
        public bool IsClosing
        {
            get { return false; }
        }
    }
}
