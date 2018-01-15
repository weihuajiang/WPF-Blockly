using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class VariableDeclarationcs : Variable
    {

        public object Value
        {
            get;
            set;
        }

        public Completion Execute(ExecutionEnvironment enviroment)
        {
            throw new NotImplementedException();
        }

        public string Name
        {
            get;
            set;
        }

        public string Type
        {
            get;
            set;
        }
    }
}
