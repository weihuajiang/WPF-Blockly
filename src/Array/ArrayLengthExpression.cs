using ScratchNet.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class ArrayLengthExpression : Expression
    {
        public Expression Array { get; set; }
        public override string ReturnType => "number";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new ExpressionDescriptor(this, "Array", "any") { NothingAllowed = true });
                desc.Add(new TextItemDescriptor(this, ".length"));
                return desc;
            }
        }

        public override string Type => "ArrayLengthExpression";

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Array == null )
                return Completion.Exception(Language.NullException, this);
            var a = Array.Execute(enviroment);
            if (!a.IsValue)
                return a;
            if (a.ReturnValue is object[])
            {
                object[] arra = a.ReturnValue as object[];
                return new Completion(arra.Length);
            }
            else if(a.ReturnValue is object[,])
            {
                object[,] arra = a.ReturnValue as object[,];
                return new Completion(arra.Length);
            }
            return Completion.Exception(Language.NotArrayException, Array);
        }
    }
}
