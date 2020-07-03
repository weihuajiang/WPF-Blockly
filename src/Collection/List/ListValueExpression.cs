using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class ListValueExpression : Expression, IAssignment
    {
        public Expression List { get; set; }
        public Expression Index { get; set; }
        public override string ReturnType => "any";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new ExpressionDescriptor(this, "List", "any") { NothingAllowed = true });
                desc.Add(new TextItemDescriptor(this, "["));
                desc.Add(new ExpressionDescriptor(this, "Index", "number") );
                desc.Add(new TextItemDescriptor(this, "]"));
                return desc;
            }
        }

        public override string Type => "ListValueExpression";

        public Completion Assign(ExecutionEnvironment enviroment, object value)
        {
            if (List == null || Index == null)
                return Completion.Exception("Null Exception", this);
            var a = List.Execute(enviroment);
            if (!a.IsValue)
                return a;
            if (a.ReturnValue is Dictionary<object, object>)
            {
                var i = Index.Execute(enviroment);
                if (!i.IsValue)
                    return i;
                if ((i.ReturnValue ==null))
                    return Completion.Exception("dictionary key can not be null", Index);
                var iv = i.ReturnValue;
                Dictionary<object, object> d = a.ReturnValue as Dictionary<object, object>;
                d[iv] = value;
                return new Completion(value);
            }
            else
            {
                var i = Index.Execute(enviroment);
                if (!i.IsValue)
                    return i;
                if (!(i.ReturnValue is int))
                    return Completion.Exception("Only integer is accepted", Index);
                var iv = (int)i.ReturnValue;
                if (a.ReturnValue is object[])
                {
                    object[] arra = a.ReturnValue as object[];
                    if (iv < 0 && iv >= arra.Length)
                    {
                        return Completion.Exception("value " + iv + " is out of array index", Index);
                    }
                    arra[iv] = value;
                    return new Completion(value);
                }
                else if (a.ReturnValue is List<object>)
                {
                    List<object> arra = a.ReturnValue as List<object>;
                    if (iv < 0 && iv >= arra.Count)
                    {
                        return Completion.Exception("value " + iv + " is out of list index", Index);
                    }
                    arra[iv] = value;
                    return new Completion(value);
                }
                return Completion.Exception("value is not an array, list or dictionary", List);
            }
        }

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (List == null || Index == null)
                return Completion.Exception("Null Exception", this);
            var a = List.Execute(enviroment);
            if (!a.IsValue)
                return a;
            if (a.ReturnValue is Dictionary<object, object>)
            {
                var i = Index.Execute(enviroment);
                if (!i.IsValue)
                    return i;
                if ((i.ReturnValue == null))
                    return Completion.Exception("dictionary key can not be null", Index);
                var iv = i.ReturnValue;
                Dictionary<object, object> d = a.ReturnValue as Dictionary<object, object>;
                if (d.ContainsKey(iv))
                    return new Completion(d[iv]);
                return Completion.Void; ;
            }
            else
            {
                var i = Index.Execute(enviroment);
                if (!i.IsValue)
                    return i;
                if (!(i.ReturnValue is int))
                    return Completion.Exception("Only integer is accepted", Index);
                var iv = (int)i.ReturnValue;
                if (a.ReturnValue is object[])
                {
                    object[] arra = a.ReturnValue as object[];
                    if (iv < 0 && iv >= arra.Length)
                    {
                        return Completion.Exception("value " + iv + " is out of array index", Index);
                    }
                    return new Completion(arra[iv]);
                }
                else if (a.ReturnValue is List<object>)
                {
                    List<object> arra = a.ReturnValue as List<object>;
                    if (iv < 0 && iv >= arra.Count)
                    {
                        return Completion.Exception("value " + iv + " is out of list index", Index);
                    }
                    return new Completion(arra[iv]);
                }
                return Completion.Exception("value is not an array, list or dictionary", List);
            }
        }
    }
}
