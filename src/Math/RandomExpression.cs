using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class RandomExpression : Expression
    {
        public override string ReturnType
        {
            get { return "number"; }
        }
        public Expression Min { get; set; } = new Literal("min");
        public Expression Max { get; set; } = new Literal("max");
        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Min == null)
                return Completion.Exception("parameter of random can not be null", Min);
            if (Max == null)
                return Completion.Exception("parameter of random can not be null", Max);
            var left = Min.Execute(enviroment);
            if (left.Type != CompletionType.Value)
                return left;
            var right = Max.Execute(enviroment);
            if (right.Type != CompletionType.Value)
                return right;
            if (right.Type != CompletionType.Value)
                return right;
            if (!TypeConverters.IsNumber(left.ReturnValue))
            {
                return Completion.Exception("value is not a number", Min);
            }
            if (!TypeConverters.IsNumber(right.ReturnValue))
            {
                return Completion.Exception("value is not a number", Max);
            }
            Type T = TypeConverters.GetMaxTypes(left.ReturnValue, right.ReturnValue);
            if (T == null)
                return Completion.Exception("Only nuber can accepted", this);

            try
            {
                var l = TypeConverters.GetValue<int>(left.ReturnValue);
                var r = TypeConverters.GetValue<int>(right.ReturnValue);
                return new Completion(random.Next(l, r));
            }
            catch (Exception e)
            {
                return Completion.Exception(e.Message, this);
            }
        }
        static Random random = new Random();
        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "random("));
                desc.Add(new ExpressionDescriptor(this, "Min", "number") { IsOnlyNumberAllowed = true });
                desc.Add(new TextItemDescriptor(this, " , "));
                desc.Add(new ExpressionDescriptor(this, "Max", "number") { IsOnlyNumberAllowed = true });
                desc.Add(new TextItemDescriptor(this, ")"));
                return desc;
            }
        }
        public override string Type
        {
            get { return "RandomExpression"; }
        }
        public bool IsClosing
        {
            get { return false; }
        }
    }

}
