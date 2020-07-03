using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class VariableDeclarationcs : Variable
    {

        public override object Value
        {
            get;
            set;
        }

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (string.IsNullOrEmpty(Name))
                return new Completion(Properties.Language.VariableNameException, CompletionType.Exception);
            enviroment.RegisterValue(Name, Value);
            return new Completion(Value, CompletionType.Value);
        }

        public override string Name
        {
            get;
            set;
        }

        public override string Type
        {
            get;
            set;
        }
    }
}
