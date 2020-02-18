using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class RandomExpression : Expression
    {
        public string ReturnType
        {
            get { return "number"; }
        }
        public Expression Min { get; set; }
        public Expression Max { get; set; }
        public Completion Execute(ExecutionEnvironment enviroment)
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
            Type T = TypeConverters.GetNumberTypes(left.ReturnValue, right.ReturnValue);
            if (T == null)
                return Completion.Exception("Only nuber can accepted", this);

            try
            {
                var l = TypeConverters.GetValue<int>(left.ReturnValue);
                var r = TypeConverters.GetValue<int>(right.ReturnValue);
                return new Completion(new Random(1437).Next(l, r));
            }
            catch (Exception e)
            {
                return Completion.Exception(e.Message, this);
            }
        }

        public Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new ExpressionDescriptor(this, "Min", "number"));
                //desc.Add(new TextItemDescriptor(this, "和"));
                desc.Add(new TextItemDescriptor(this, Properties.Resources.random1));
                desc.Add(new ExpressionDescriptor(this, "Max", "number"));
                //desc.Add(new TextItemDescriptor(this, "之间的随机数"));
                desc.Add(new TextItemDescriptor(this, Properties.Resources.random2));
                return desc;
            }
        }
        public string Type
        {
            get { return "RandomExpression"; }
        }
        public bool IsClosing
        {
            get { return false; }
        }
    }

}
