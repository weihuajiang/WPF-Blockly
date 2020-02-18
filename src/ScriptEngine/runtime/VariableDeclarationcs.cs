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
            if (string.IsNullOrEmpty(Name))
                return new Completion("Variable name can not be null", CompletionType.Exception);
            enviroment.RegisterValue(Name, Value);
            return new Completion(Value, CompletionType.Value);
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
