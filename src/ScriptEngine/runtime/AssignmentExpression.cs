using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public enum AssignmentOperator
    {
        AddEqual,
        MinusEqual,
        MulitiplyEqual,
        DivideEqual,
        ModEqual,
        Equal,
        BitAndEqual,
        BitOrEqual,
        BitRightShiftEqual,
        BitLeftShiftEqual,
        BitExclusiveOrEqual
    }
    public class AssignmentExpression : Expression
    {
        public AssignmentExpression()
        {

        }
        public Expression Left { get; set; }
        public Expression Right { get; set; }
        public AssignmentOperator Operator { get; set; } = AssignmentOperator.Equal;
        public override string ReturnType
        {
            get { return "number|string|boolean"; }
        }
        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Right == null)
                return Completion.Exception(Properties.Language.NoValueAssigned, this);
            var right = Right.Execute(enviroment);
            if (right.Type != CompletionType.Value)
                return right;
            if (Left == null)
                return right;
            if (Left is IAssignment)
            {
                try
                {
                    if (Operator == AssignmentOperator.Equal)
                        return (Left as IAssignment).Assign(enviroment, right.ReturnValue);
                    var l = Left.Execute(enviroment);
                    if (!l.IsValue)
                        return l;
                    if (l.ReturnValue == null)
                        return Completion.Exception(Properties.Language.VariableNullException, Left);
                    Type lt = l.ReturnValue.GetType();

                    if (l.ReturnValue is string || right.ReturnValue is string)
                    {
                        if (Operator != AssignmentOperator.AddEqual)
                            return Completion.Exception(Properties.Language.StringOnlySupport, this);
                        return (Left as IAssignment).Assign(enviroment, l.ReturnValue + "" + right.ReturnValue);
                    }
                    if (!TypeConverters.IsNumber(l.ReturnValue))
                        return Completion.Exception(Properties.Language.NotNumber, Left);
                    if (!TypeConverters.IsNumber(right.ReturnValue))
                        return Completion.Exception(Properties.Language.NotNumber, Right);
                    Type maxType = TypeConverters.GetMaxTypes(l.ReturnValue, right.ReturnValue);
                    Console.Write(maxType);
                    if (maxType.Equals(typeof(char)))
                    {
                        var lint = TypeConverters.GetValue<char>(l.ReturnValue);
                        var rint = TypeConverters.GetValue<char>(right.ReturnValue);
                        if (Operator == AssignmentOperator.AddEqual)
                            return (Left as IAssignment).Assign(enviroment, lint + rint);
                        if (Operator == AssignmentOperator.MinusEqual)
                            return (Left as IAssignment).Assign(enviroment, lint - rint);
                        if (Operator == AssignmentOperator.MulitiplyEqual)
                            return (Left as IAssignment).Assign(enviroment, lint * rint);
                        if (Operator == AssignmentOperator.DivideEqual)
                            return (Left as IAssignment).Assign(enviroment, lint / rint);
                        if (Operator == AssignmentOperator.ModEqual)
                            return (Left as IAssignment).Assign(enviroment, lint % rint);
                        if (Operator == AssignmentOperator.BitAndEqual)
                            return (Left as IAssignment).Assign(enviroment, lint & rint);
                        if (Operator == AssignmentOperator.BitExclusiveOrEqual)
                            return (Left as IAssignment).Assign(enviroment, lint ^ rint);
                        if (Operator == AssignmentOperator.BitLeftShiftEqual)
                            return (Left as IAssignment).Assign(enviroment, lint << rint);
                        if (Operator == AssignmentOperator.BitOrEqual)
                            return (Left as IAssignment).Assign(enviroment, lint | rint);
                        if (Operator == AssignmentOperator.BitRightShiftEqual)
                            return (Left as IAssignment).Assign(enviroment, lint >> rint);
                        return Completion.Exception(Properties.Language.UnknowException, this);
                    }
                    if (Operator == AssignmentOperator.BitAndEqual || Operator == AssignmentOperator.BitExclusiveOrEqual || Operator == AssignmentOperator.BitLeftShiftEqual ||
                        Operator == AssignmentOperator.BitOrEqual || Operator == AssignmentOperator.BitRightShiftEqual)
                    {
                        int lint = TypeConverters.GetValue<int>(l.ReturnValue);
                        int rint = TypeConverters.GetValue<int>(right.ReturnValue);
                        if (Operator == AssignmentOperator.BitAndEqual)
                            return (Left as IAssignment).Assign(enviroment, lint & rint);
                        if (Operator == AssignmentOperator.BitExclusiveOrEqual)
                            return (Left as IAssignment).Assign(enviroment, lint ^ rint);
                        if (Operator == AssignmentOperator.BitLeftShiftEqual)
                            return (Left as IAssignment).Assign(enviroment, lint << rint);
                        if (Operator == AssignmentOperator.BitOrEqual)
                            return (Left as IAssignment).Assign(enviroment, lint | rint);
                        if (Operator == AssignmentOperator.BitRightShiftEqual)
                            return (Left as IAssignment).Assign(enviroment, lint >> rint);
                        return Completion.Exception(Properties.Language.UnknowException, this);
                    }
                    if (maxType.Equals(typeof(int)))
                    {
                        int lint = TypeConverters.GetValue<int>(l.ReturnValue);
                        int rint = TypeConverters.GetValue<int>(right.ReturnValue);
                        if (Operator == AssignmentOperator.AddEqual)
                            return (Left as IAssignment).Assign(enviroment, lint + rint);
                        if (Operator == AssignmentOperator.MinusEqual)
                            return (Left as IAssignment).Assign(enviroment, lint - rint);
                        if (Operator == AssignmentOperator.MulitiplyEqual)
                            return (Left as IAssignment).Assign(enviroment, lint * rint);
                        if (Operator == AssignmentOperator.DivideEqual)
                            return (Left as IAssignment).Assign(enviroment, lint / rint);
                        if (Operator == AssignmentOperator.ModEqual)
                            return (Left as IAssignment).Assign(enviroment, lint % rint);
                        return Completion.Exception(Properties.Language.UnknowException, this);
                    }
                    else if (maxType.Equals(typeof(float)))
                    {
                        float lint = TypeConverters.GetValue<float>(l.ReturnValue);
                        float rint = TypeConverters.GetValue<int>(right.ReturnValue);
                        if (Operator == AssignmentOperator.AddEqual)
                            return (Left as IAssignment).Assign(enviroment, lint + rint);
                        if (Operator == AssignmentOperator.MinusEqual)
                            return (Left as IAssignment).Assign(enviroment, lint - rint);
                        if (Operator == AssignmentOperator.MulitiplyEqual)
                            return (Left as IAssignment).Assign(enviroment, lint * rint);
                        if (Operator == AssignmentOperator.DivideEqual)
                            return (Left as IAssignment).Assign(enviroment, lint / rint);
                        if (Operator == AssignmentOperator.ModEqual)
                            return (Left as IAssignment).Assign(enviroment, lint % rint);
                        return Completion.Exception(Properties.Language.UnknowException, this);
                    }
                    else
                    {
                        double lint = TypeConverters.GetValue<double>(l.ReturnValue);
                        double rint = TypeConverters.GetValue<double>(right.ReturnValue);
                        if (Operator == AssignmentOperator.AddEqual)
                            return (Left as IAssignment).Assign(enviroment, lint + rint);
                        if (Operator == AssignmentOperator.MinusEqual)
                            return (Left as IAssignment).Assign(enviroment, lint - rint);
                        if (Operator == AssignmentOperator.MulitiplyEqual)
                            return (Left as IAssignment).Assign(enviroment, lint * rint);
                        if (Operator == AssignmentOperator.DivideEqual)
                            return (Left as IAssignment).Assign(enviroment, lint / rint);
                        if (Operator == AssignmentOperator.ModEqual)
                            return (Left as IAssignment).Assign(enviroment, lint % rint);
                        return Completion.Exception(Properties.Language.UnknowException, this);
                    }
                }catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                    return Completion.Exception(e.Message, this);
                }
            }
            else
            {
                return Completion.Exception(Properties.Language.InvalidBeforeEqual, Left);
            }
        }

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new ExpressionDescriptor(this, "Left", "number|string|boolean") { NothingAllowed = true }); 
                desc.Add(new TextItemDescriptor(this, " "));
                desc.Add(new SelectionItemDescriptor(this, "Operator", new object[] { "=", "+=", "-=", "*=", 
                    "/=", "%=", 
                    "&=", "|=", "^=", 
                    "<<=", ">>=" },
                    new object[] {
                        AssignmentOperator.Equal, AssignmentOperator.AddEqual, AssignmentOperator.MinusEqual, AssignmentOperator.MulitiplyEqual,
                        AssignmentOperator.DivideEqual, AssignmentOperator.ModEqual,
                        AssignmentOperator.BitAndEqual, AssignmentOperator.BitOrEqual, AssignmentOperator.BitExclusiveOrEqual,
                        AssignmentOperator.BitLeftShiftEqual, AssignmentOperator.BitRightShiftEqual
                    }));
                desc.Add(new TextItemDescriptor(this, " "));
                desc.Add(new ExpressionDescriptor(this, "Right", "number|string|boolean") { IsOnlyNumberAllowed = false });
                return desc;

            }
        }

        public override string Type
        {
            get { return "VariableAssignmentExpression"; }
        }


    }
}
