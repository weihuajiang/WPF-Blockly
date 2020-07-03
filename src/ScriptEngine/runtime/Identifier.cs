using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class Identifier:Expression, IAssignment
    {
        public Identifier()
        {

        }
        public Identifier(string variable)
        {
            Variable = variable;
        }
        public String Variable { get; set; }
        public string VarType { get; set; }
        public override string ReturnType
        {
            get { return VarType; }
        }

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            try
            {
                var value = enviroment.GetValue(Variable);
                return new Completion(value);
            }catch(Exception e)
            {
                return Completion.Exception(string.Format(Properties.Language.VariableNotDefined, Variable), this);
            }
        }

        public Completion Assign(ExecutionEnvironment environemtn, object value)
        {
            try
            {
                environemtn.SetValue(Variable, value);
                return new Completion(value);
            }catch(Exception e)
            {
                return Completion.Exception(string.Format(Properties.Language.VariableNotDefined, Variable), this);
            }
        }

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, Variable));
                return desc;
            }
        }

        public override string Type
        {
            get { return VarType; }
        }
        
    }
}
