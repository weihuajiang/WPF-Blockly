using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class AssignmentExpression : Expression
    {
        public AssignmentExpression()
        {
            
        }
        public String Left { get; set; }
        public Expression Right { get; set; }
        public string ReturnType
        {
            get { return "number|string|boolean"; }
        }

        public Completion Execute(ExecutionEnvironment enviroment)
        {
            return Completion.Void;
        }

        public Descriptor Descriptor
        {
            get {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "Set "));
                desc.Add(new VariableDescriptor(this, "Left"));
                desc.Add(new TextItemDescriptor(this, " to "));
                desc.Add(new ExpressionDescriptor(this, "Right", "number|string|boolean"));
                return desc;
            }
        }

        public string Type
        {
            get { return "VariableAssignmentExpression"; }
        }
    }
}
