using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class AssignmentStatement : Statement, Expression
    {
        public AssignmentStatement()
        {
            
        }
        public Expression Left { get; set; }
        public Expression Right { get; set; }
        public string ReturnType
        {
            get { return "number|string|boolean"; }
        }

        public Completion Execute(ExecutionEnvironment enviroment)
        {
            if (Right == null)
                return Completion.Void;
            var right = Right.Execute(enviroment);
            if (right.Type != CompletionType.Value)
                return right;
            if (Left == null)
                return right;
            if(Left  is Identifier)
            {
                enviroment.SetValue(((Identifier)Left).Variable, right.ReturnValue);
                return right;
            }
            else
            {
                return new Completion("invliad format before =", CompletionType.Exception);
            }
            //var left = Left.Execute(enviroment);
            
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

        public BlockDescriptor BlockDescriptor
        {
            get { return null; }
        }

        public bool IsClosing
        {
            get { return false; }
        }

    }
}
