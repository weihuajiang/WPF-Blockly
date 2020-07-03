using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class VariableDeclarationExpression : Expression
    {
        public override string ReturnType => "any";
        public bool CanAssignValue { get; set; } = true;

        Expression var;
        public Expression Variable
        {
            get
            {
                return var;
            }
            set
            {
                var = value;
                string name = null;
                if (var is Literal)
                    name = (var as Literal).Raw;
                else if (var is Identifier)
                    name = (var as Identifier).Variable;
                if (nameDescriptor != null)
                {
                    nameDescriptor.Name = name;
                    nameDescriptor.OnPropertyChanged("Name");
                }
            }
        }
        public Expression Value { get; set; }

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Variable == null)
                return Completion.Exception(Properties.Language.VariableNameException, this);
            if (!(Variable is Identifier))
                return Completion.Exception(Properties.Language.OnlyVariableAccept, Variable);
            string name = (Variable as Identifier).Variable;
            if (Value == null)
            {
                enviroment.RegisterValue(name, null);
                return Completion.Void;
            }
            else
            {
                var c = Value.Execute(enviroment);
                if (!c.IsValue)
                    return c;
                enviroment.RegisterValue(name, c.ReturnValue);
                return c;
            }
        }
        VariableDeclarationDescription nameDescriptor = null;
        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "var  ", true));
                string name = null;
                if (Variable is Literal)
                    name = (Variable as Literal).Raw;
                else if (Variable is Identifier)
                    name = (Variable as Identifier).Variable;

                if (nameDescriptor == null)
                    nameDescriptor = new VariableDeclarationDescription(this, name, "number|string|boolean");
                desc.Add(nameDescriptor);
                if (CanAssignValue)
                {
                    desc.Add(new TextItemDescriptor(this, " = "));
                    desc.Add(new ExpressionDescriptor(this, "Value", "any"));
                }
                return desc;
            }
        }

        public override string Type
        {
            get { return "VariableAssignmentExpression"; }
        }

    }
}
