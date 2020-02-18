using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class Literal : Expression
    {
        public string ReturnType
        {
            get { return ""; }
        }

        public Completion Execute(ExecutionEnvironment enviroment)
        {
            throw new NotImplementedException();
        }

        public Descriptor Descriptor
        {
            get { throw new NotImplementedException(); }
        }

        public string Type
        {
            get { return "Literal"; }
        }
        public object Value
        {
            get;
            set;
        }
        public string Raw
        {
            get;
            set;
        }
    }
}
