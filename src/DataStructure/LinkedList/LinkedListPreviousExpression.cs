using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class LinkedListPreviousExpression : Expression, IAssignment
    {
        public Expression Node { get; set; }
        public override string ReturnType => "any";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new ExpressionDescriptor(this, "Node", "any") { NothingAllowed = true });
                desc.Add(new TextItemDescriptor(this, ".previous"));
                return desc;
            }
        }

        public override string Type => "LinkedListValueExpression";

        public Completion Assign(ExecutionEnvironment enviroment, object value)
        {
            if (value != null && !(value is LinkedListEx))
                return Completion.Exception("value is not a linked list type", this);
            if (Node == null)
                return Completion.Exception("Null Exception", this);
            if ((Node is Identifier))
            {
                string var = (Node as Identifier).Variable;
                if (!enviroment.HasValue(var))
                {
                    return Completion.Exception("varaible was not defined", Node);
                }
                LinkedListEx tree = enviroment.GetValue(var) as LinkedListEx;
                if (tree == null)
                {
                    return Completion.Exception("variable is not linked list", Node);
                }
                tree.Previous = value as LinkedListEx;
                return new Completion(value);
            }
            else
            {
                var c = Node.Execute(enviroment);
                if (!c.IsValue)
                    return c;
                if (c.ReturnValue is LinkedListEx)
                {
                    (c.ReturnValue as LinkedListEx).Previous = value as LinkedListEx;
                    return new Completion(value);
                }
                else
                    return Completion.Exception("value is not linked list", Node);
            }
        }

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Node == null)
                return Completion.Exception("Null Exception", this);
            if (Node is Identifier)
            {
                string var = (Node as Identifier).Variable;
                if (!enviroment.HasValue(var))
                {
                    return Completion.Exception("varaible was not defined", Node);
                }
                LinkedListEx tree = enviroment.GetValue(var) as LinkedListEx;
                if (tree == null)
                {
                    return Completion.Exception("variable is not linked list", Node);
                }
                return new Completion(tree.Previous);
            }
            else
            {
                var c = Node.Execute(enviroment);
                if (!c.IsValue)
                    return c;
                if (c.ReturnValue is LinkedListEx)
                {
                    return new Completion((c.ReturnValue as LinkedListEx).Previous);
                }
                else
                    return Completion.Exception("value is not linked list", Node);
            }
        }
    }
}
