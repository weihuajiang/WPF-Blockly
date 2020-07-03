using ScratchNet.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class SinExpression : Expression
    {
        public Expression Degree { get; set; }
        public override string ReturnType => "number";

        public override Descriptor Descriptor
        {
            get
            {

                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "sin("));
                desc.Add(new ExpressionDescriptor(this, "Degree", "number") { IsOnlyNumberAllowed = true });
                desc.Add(new TextItemDescriptor(this, ")"));
                return desc;
            }
        }

        public override string Type => "SinExpression";

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Degree == null)
                return Completion.Exception(Language.NullException, this);
            var c = Degree.Execute(enviroment);
            if (!c.IsValue)
                return c;
            if (!TypeConverters.IsNumber(c.ReturnValue))
                return Completion.Exception(Language.NotNumberException, Degree);
            double d = TypeConverters.GetValue<double>(c.ReturnValue) *  Math.PI/180;
            return new Completion(Math.Sin(d));
        }
    }
    public class CosExpression : Expression
    {
        public Expression Degree { get; set; }
        public override string ReturnType => "number";

        public override Descriptor Descriptor
        {
            get
            {

                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "cos("));
                desc.Add(new ExpressionDescriptor(this, "Degree", "number") { IsOnlyNumberAllowed = true });
                desc.Add(new TextItemDescriptor(this, ")"));
                return desc;
            }
        }

        public override string Type => "CosExpression";

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Degree == null)
                return Completion.Exception(Language.NullException, this);
            var c = Degree.Execute(enviroment);
            if (!c.IsValue)
                return c;
            if (!TypeConverters.IsNumber(c.ReturnValue))
                return Completion.Exception(Language.NotNumberException, Degree);
            double d = TypeConverters.GetValue<double>(c.ReturnValue) * Math.PI / 180;
            return new Completion(Math.Cos(d));
        }
    }
    public class TanExpression : Expression
    {
        public Expression Degree { get; set; }
        public override string ReturnType => "number";

        public override Descriptor Descriptor
        {
            get
            {

                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "tan("));
                desc.Add(new ExpressionDescriptor(this, "Degree", "number") { IsOnlyNumberAllowed = true });
                desc.Add(new TextItemDescriptor(this, ")"));
                return desc;
            }
        }

        public override string Type => "TanExpression";

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Degree == null)
                return Completion.Exception(Language.NullException, this);
            var c = Degree.Execute(enviroment);
            if (!c.IsValue)
                return c;
            if (!TypeConverters.IsNumber(c.ReturnValue))
                return Completion.Exception(Language.NotNumberException, Degree);
            double d = TypeConverters.GetValue<double>(c.ReturnValue) * Math.PI / 180;
            return new Completion(Math.Tan(d));
        }
    }
    public class CtgExpression : Expression
    {
        public Expression Degree { get; set; }
        public override string ReturnType => "number";

        public override Descriptor Descriptor
        {
            get
            {

                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "ctg("));
                desc.Add(new ExpressionDescriptor(this, "Degree", "number") { IsOnlyNumberAllowed = true });
                desc.Add(new TextItemDescriptor(this, ")"));
                return desc;
            }
        }

        public override string Type => "CtgExpression";

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Degree == null)
                return Completion.Exception(Language.NullException, this);
            var c = Degree.Execute(enviroment);
            if (!c.IsValue)
                return c;
            if (!TypeConverters.IsNumber(c.ReturnValue))
                return Completion.Exception(Language.NotNumberException, Degree);
            double d = TypeConverters.GetValue<double>(c.ReturnValue) * Math.PI / 180;
            return new Completion(1/Math.Tan(d));
        }
    }
    public class ASinExpression : Expression
    {
        public Expression Value { get; set; }
        public override string ReturnType => "number";

        public override Descriptor Descriptor
        {
            get
            {

                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "asin("));
                desc.Add(new ExpressionDescriptor(this, "Value", "number") { IsOnlyNumberAllowed = true });
                desc.Add(new TextItemDescriptor(this, ")"));
                return desc;
            }
        }

        public override string Type => "ASinExpression";

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Value == null)
                return Completion.Exception(Language.NullException, this);
            var c = Value.Execute(enviroment);
            if (!c.IsValue)
                return c;
            if (!TypeConverters.IsNumber(c.ReturnValue))
                return Completion.Exception(Language.NotNumberException, Value);
            double d = TypeConverters.GetValue<double>(c.ReturnValue);
            return new Completion(Math.Asin(d)*180/Math.PI);
        }
    }
    public class ACosExpression : Expression
    {
        public Expression Value { get; set; }
        public override string ReturnType => "number";

        public override Descriptor Descriptor
        {
            get
            {

                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "acos("));
                desc.Add(new ExpressionDescriptor(this, "Value", "number") { IsOnlyNumberAllowed = true });
                desc.Add(new TextItemDescriptor(this, ")"));
                return desc;
            }
        }

        public override string Type => "ACosExpression";

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Value == null)
                return Completion.Exception(Language.NullException, this);
            var c = Value.Execute(enviroment);
            if (!c.IsValue)
                return c;
            if (!TypeConverters.IsNumber(c.ReturnValue))
                return Completion.Exception(Language.NotNumberException, Value);
            double d = TypeConverters.GetValue<double>(c.ReturnValue);
            return new Completion(Math.Acos(d) * 180 / Math.PI);
        }
    }
    public class ATanExpression : Expression
    {
        public Expression Value { get; set; }
        public override string ReturnType => "number";

        public override Descriptor Descriptor
        {
            get
            {

                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "atan("));
                desc.Add(new ExpressionDescriptor(this, "Value", "number") { IsOnlyNumberAllowed = true });
                desc.Add(new TextItemDescriptor(this, ")"));
                return desc;
            }
        }

        public override string Type => "ATanExpression";

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Value == null)
                return Completion.Exception(Language.NullException, this);
            var c = Value.Execute(enviroment);
            if (!c.IsValue)
                return c;
            if (!TypeConverters.IsNumber(c.ReturnValue))
                return Completion.Exception(Language.NotNumberException, Value);
            double d = TypeConverters.GetValue<double>(c.ReturnValue);
            return new Completion(Math.Atan(d) * 180 / Math.PI);
        }
    }
    public class ACtgExpression : Expression
    {
        public Expression Value { get; set; }
        public override string ReturnType => "number";

        public override Descriptor Descriptor
        {
            get
            {

                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "actg("));
                desc.Add(new ExpressionDescriptor(this, "Value", "number") { IsOnlyNumberAllowed = true });
                desc.Add(new TextItemDescriptor(this, ")"));
                return desc;
            }
        }

        public override string Type => "ACtgExpression";

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Value == null)
                return Completion.Exception(Language.NullException, this);
            var c = Value.Execute(enviroment);
            if (!c.IsValue)
                return c;
            if (!TypeConverters.IsNumber(c.ReturnValue))
                return Completion.Exception(Language.NotNumberException, Value);
            double d = TypeConverters.GetValue<double>(c.ReturnValue);
            return new Completion(Math.Atan(1/d) * 180 / Math.PI);
        }
    }
}
