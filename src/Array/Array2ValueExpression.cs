using ScratchNet.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class Array2ValueExpression : Expression, IAssignment
    {
        public Expression Array { get; set; }
        public Expression Index1 { get; set; }
        public Expression Index2 { get; set; }
        public override string ReturnType => "any";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new ExpressionDescriptor(this, "Array", "any") { NothingAllowed = true });
                desc.Add(new TextItemDescriptor(this, "["));
                desc.Add(new ExpressionDescriptor(this, "Index1", "number") { IsOnlyNumberAllowed = true });
                desc.Add(new TextItemDescriptor(this, "]["));
                desc.Add(new ExpressionDescriptor(this, "Index2", "number") { IsOnlyNumberAllowed = true });
                desc.Add(new TextItemDescriptor(this, "]"));
                return desc;
            }
        }

        public override string Type => "ArrayValueExpression";

        public Completion Assign(ExecutionEnvironment enviroment, object value)
        {
            if (Array == null || Index1 == null || Index2 == null)
                return Completion.Exception(Language.NullException, this);
            var a = Array.Execute(enviroment);
            if (!a.IsValue)
                return a;
            if (!(a.ReturnValue is object[,]))
                return Completion.Exception(Language.NotNumberException, Array);

            object[,] arra = a.ReturnValue as object[,];

            var i = Index1.Execute(enviroment);
            if (!i.IsValue)
                return i;
            if (!(i.ReturnValue is int))
                return Completion.Exception(Language.NotNumberException, Index1);
            var iv = (int)i.ReturnValue;

            var r = Index2.Execute(enviroment);
            if (!r.IsValue)
                return r;
            if (!(r.ReturnValue is int))
                return Completion.Exception(Language.NotNumberException, Index2);
            var jv = (int)r.ReturnValue;
            if (iv < 0 && iv >= arra.GetLength(0))
            {
                return Completion.Exception(Language.IndexOutOfRangeException, Index1);
            }
            if (jv < 0 && jv >= arra.GetLength(1))
            {
                return Completion.Exception(Language.IndexOutOfRangeException, Index2);
            }
            try
            {
                arra[iv, jv] = value;
            }
            catch (Exception e)
            {
                return Completion.Exception(e.Message, this);
            }
            return new Completion(value);
        }

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Array == null || Index1 == null || Index2 == null)
                return Completion.Exception(Language.NullException, this);
            var a = Array.Execute(enviroment);
            if (!a.IsValue)
                return a;
            if (!(a.ReturnValue is object[,]))
                return Completion.Exception(Language.NotNumberException, Array);

            object[,] arra = a.ReturnValue as object[,];

            var i = Index1.Execute(enviroment);
            if (!i.IsValue)
                return i;
            if (!(i.ReturnValue is int))
                return Completion.Exception(Language.NotNumberException, Index1);
            var iv = (int)i.ReturnValue;

            var r = Index2.Execute(enviroment);
            if (!r.IsValue)
                return r;
            if (!(r.ReturnValue is int))
                return Completion.Exception(Language.NotNumberException, Index2);
            var jv = (int)r.ReturnValue;
            if (iv < 0 && iv >= arra.GetLength(0))
            {
                return Completion.Exception(Language.IndexOutOfRangeException, Index1);
            }
            if (jv < 0 && jv >= arra.GetLength(1))
            {
                return Completion.Exception(Language.IndexOutOfRangeException, Index2);
            }
            try
            {
                return new Completion(arra[iv, jv]);
            }catch(Exception e)
            {
                return Completion.Exception(e.Message, this);
            }
        }
    }
}
