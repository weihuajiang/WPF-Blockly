using ScratchNet.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class ArrayValueExpression : Expression, IAssignment
    {
        public Expression Array { get; set; }
        public Expression Index { get; set; }
        public override string ReturnType => "any";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new ExpressionDescriptor(this, "Array", "any") { NothingAllowed = true });
                desc.Add(new TextItemDescriptor(this, "["));
                desc.Add(new ExpressionDescriptor(this, "Index", "number"));
                desc.Add(new TextItemDescriptor(this, "]"));
                return desc;
            }
        }

        public override string Type => "ListValueExpression";

        public Completion Assign(ExecutionEnvironment enviroment, object value)
        {
            if (Array == null || Index == null)
                return Completion.Exception(Language.NullException, this);
            var a = Array.Execute(enviroment);
            if (!a.IsValue)
                return a;
            if (a.ReturnValue is Dictionary<object, object>)
            {
                var i = Index.Execute(enviroment);
                if (!i.IsValue)
                    return i;
                if ((i.ReturnValue == null))
                    return Completion.Exception(Language.ValueNullException, Index);
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
                    return Completion.Exception(Language.NotNumberException, Index);
                var iv = (int)i.ReturnValue;
                if (a.ReturnValue is object[])
                {
                    object[] arra = a.ReturnValue as object[];
                    if (iv < 0 && iv >= arra.Length)
                    {
                        return Completion.Exception(Language.IndexOutOfRangeException, Index);
                    }
                    arra[iv] = value;
                    return new Completion(value);
                }
                else if (a.ReturnValue is List<object>)
                {
                    List<object> arra = a.ReturnValue as List<object>;
                    if (iv < 0 && iv >= arra.Count)
                    {
                        return Completion.Exception(Language.IndexOutOfRangeException, Index);
                    }
                    arra[iv] = value;
                    return new Completion(value);
                }
                return Completion.Exception(Language.NotCollectionException, Array);
            }
        }

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Array == null || Index == null)
                return Completion.Exception(Language.NullException, this);
            var a = Array.Execute(enviroment);
            if (!a.IsValue)
                return a;
            if (a.ReturnValue is Dictionary<object, object>)
            {
                var i = Index.Execute(enviroment);
                if (!i.IsValue)
                    return i;
                if ((i.ReturnValue == null))
                    return Completion.Exception(Language.ValueNullException, Index);
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
                    return Completion.Exception(Language.NotNumberException, Index);
                var iv = (int)i.ReturnValue;
                if (a.ReturnValue is object[])
                {
                    object[] arra = a.ReturnValue as object[];
                    if (iv < 0 && iv >= arra.Length)
                    {
                        return Completion.Exception(Language.IndexOutOfRangeException, Index);
                    }
                    return new Completion(arra[iv]);
                }
                else if (a.ReturnValue is List<object>)
                {
                    List<object> arra = a.ReturnValue as List<object>;
                    if (iv < 0 && iv >= arra.Count)
                    {
                        return Completion.Exception(Language.IndexOutOfRangeException, Index);
                    }
                    return new Completion(arra[iv]);
                }
                return Completion.Exception(Language.NotCollectionException, Array);
            }
        }
    }
}
