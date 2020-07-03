using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public enum MemberType
    {
        Property,
        Function,
        Field
    }
    public class MemberExpression : Expression, IAssignment
    {
        public Expression Object { get; set; }
        public Expression Member { get; set; }
        public override string ReturnType => "any";
        public string Operator { get; set; } = ".";
        public string Operator2 { get; set; } = "";
        public string GetMemberFunction { get; set; } = "";
        public string SetMemberFunction { get; set; } = "";
        public bool IsMemberRequired { get; set; } = true;
        public MemberType MemberType { get; set; } = MemberType.Function;

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new ExpressionDescriptor(this, "Object", "any") { NothingAllowed = true });
                desc.Add(new TextItemDescriptor(this, Operator));
                if(IsMemberRequired)
                    desc.Add(new ExpressionDescriptor(this, "Member", "any"));
                if (!string.IsNullOrEmpty(Operator2))
                    desc.Add(new TextItemDescriptor(this, Operator2));
                return desc;
            }
        }
        public Completion Assign(ExecutionEnvironment environment, object value)
        {
            if (string.IsNullOrEmpty(SetMemberFunction))
                return Completion.Exception("this is can not set", this);
            if (Object == null)
                return Completion.Exception("object parameter can not be null", this);
            var tc = Object.Execute(environment);
            if (!tc.IsValue)
                return tc;
            object obj = tc.ReturnValue;
            if (obj == null)
                return Completion.Exception("object value can not be null", Object);
            object m = null;
            if (IsMemberRequired)
            {
                if (Member == null)
                    return Completion.Exception("member parameter can not be null", this);
                var mc = Member.Execute(environment);
                if (!mc.IsValue)
                    return mc;
                m = mc.ReturnValue;
            }
            Type t = obj.GetType();
            try
            {
                if (MemberType == MemberType.Function)
                {
                    MethodInfo set = t.GetMethod(SetMemberFunction);
                    if (set == null)
                        return Completion.Exception("No member function for " + obj.GetType(), Member);
                    if (IsMemberRequired)
                        set.Invoke(obj, new object[] { m, value });
                    else
                    {
                        set.Invoke(obj, new object[] { value });
                    }
                    return new Completion(value);
                }
                else if (MemberType == MemberType.Property)
                {
                    PropertyInfo set = t.GetProperty(SetMemberFunction);
                    if (set == null)
                        return Completion.Exception("No member function for " + obj.GetType(), Member);
                    set.SetValue(obj, value);
                    return new Completion(value);
                }
                else
                {
                    FieldInfo set = t.GetField(SetMemberFunction);
                    if (set == null)
                        return Completion.Exception("No member function for " + obj.GetType(), Member);
                    set.SetValue(obj, value);
                    return new Completion(value);
                }
            }catch(Exception e)
            {
                return Completion.Exception(e.Message, this);
            }
        }
        public override string Type => "MemberExpression";


        protected override Completion ExecuteImpl(ExecutionEnvironment environment)
        {
            if (Object == null)
                return Completion.Exception("object parameter can not be null", this);
            var tc = Object.Execute(environment);
            if (!tc.IsValue)
                return tc;
            object obj = tc.ReturnValue;
            if (obj == null)
                return Completion.Exception("object value can not be null", Object);
            object m = null;
            if (IsMemberRequired)
            {
                if (Member == null)
                    return Completion.Exception("member parameter can not be null", this);
                var mc = Member.Execute(environment);
                if (!mc.IsValue)
                    return mc;
                m = mc.ReturnValue;
            }
            Type t = obj.GetType();
            try
            {
                if (MemberType == MemberType.Function)
                {
                    MethodInfo get = t.GetMethod(GetMemberFunction);
                    if (get == null)
                        return Completion.Exception("No member function for " + obj.GetType(), Member);
                    object value = null;
                    if (IsMemberRequired)
                        value=get.Invoke(obj, new object[] { m });
                    else
                    {
                        value=get.Invoke(obj, new object[] {  });
                    }
                    return new Completion(value);
                }
                else if (MemberType == MemberType.Property)
                {
                    PropertyInfo get = t.GetProperty(GetMemberFunction);
                    if (get == null)
                        return Completion.Exception("No member function for " + obj.GetType(), Member);
                    object value=get.GetValue(obj);
                    return new Completion(value);
                }
                else
                {
                    FieldInfo get = t.GetField(GetMemberFunction);
                    if (get == null)
                        return Completion.Exception("No member function for " + obj.GetType(), Member);
                    object value=get.GetValue(obj);
                    return new Completion(value);
                }
            }
            catch (Exception e)
            {
                return Completion.Exception(e.Message, this);
            }
        }
    }
}
