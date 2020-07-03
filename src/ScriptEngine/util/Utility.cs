using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ScratchNet
{
    public static class UtilityExtension
    {
        public static Expression Clone(this Expression exp)
        {
            if (exp == null)
                return null;
            Type t = exp.GetType();
            Expression d = Activator.CreateInstance(t) as Expression;
            PropertyInfo[] pinf = t.GetProperties();
            foreach (PropertyInfo p in pinf)
            {
                if (p.CanRead && p.CanWrite)
                {
                    object pvalue = p.GetValue(exp, null);
                    if (pvalue != null)
                        if (pvalue is Expression)
                        {
                            pvalue = Clone(pvalue as Expression);
                            p.SetValue(d, pvalue, null);
                        }
                        else if (pvalue is List<Expression>)
                        {
                            try
                            {
                                List<Expression> target = pvalue as List<Expression>;
                                List<Expression> nexps = new List<Expression>();
                                foreach (Expression e in target)
                                {
                                    nexps.Add(Clone(e));
                                }
                                p.SetValue(d, nexps, null);
                            }
                            catch (Exception ex) { Console.WriteLine(ex.Message); }
                        }
                        else if (pvalue is List<string>)
                        {
                            List<string> target = pvalue as List<string>;
                            List<string> newStr = new List<string>();
                            foreach (string pv in target)
                                newStr.Add(pv);
                            p.SetValue(d, newStr, null);
                        }
                        else
                            p.SetValue(d, pvalue, null);
                }
            }
            return d;
        }


        public static Statement Clone(this Statement st)
        {
            Type t = st.GetType();
            Statement d = Activator.CreateInstance(t) as Statement;
            PropertyInfo[] pinf = t.GetProperties();
            foreach (PropertyInfo p in pinf)
            {
                if (p.CanRead && p.CanWrite)
                {
                    object pvalue = p.GetValue(st, null);
                    if (pvalue is Expression)
                    {
                        pvalue = Clone(pvalue as Expression);
                    }
                    else if (pvalue is BlockStatement)
                    {
                        pvalue = Clone(pvalue as BlockStatement);
                    }
                    else if (pvalue is Function)
                    {
                        pvalue = Clone(pvalue as Function);
                    }
                    if (pvalue == null)
                        continue;
                    if (pvalue is List<Expression>)
                    {
                        try
                        {
                            List<Expression> target = pvalue as List<Expression>;
                            List<Expression> nexps = new List<Expression>();
                            foreach (Expression e in target)
                            {
                                nexps.Add(Clone(e));
                            }
                            p.SetValue(d, nexps, null);
                        }
                        catch (Exception ex) { }
                    }
                    else if (pvalue is List<string>)
                    {
                        List<string> target = pvalue as List<string>;
                        List<string> newStr = new List<string>();
                        foreach (string pv in target)
                            newStr.Add(pv);
                        p.SetValue(d, newStr, null);
                    }
                    else
                    {
                        p.SetValue(d, pvalue, null);
                    }

                }
            }
            return d;
        }
        public static BlockStatement Clone(this BlockStatement bs)
        {
            Type t = bs.GetType();
            BlockStatement d = Activator.CreateInstance(t) as BlockStatement;
            foreach (Statement cst in bs.Body)
            {
                d.Body.Add(Clone(cst));
            }
            return d;
        }

        public static Function Clone(this Function st)
        {
            Type t = st.GetType();
            Function d = Activator.CreateInstance(t) as Function;
            PropertyInfo[] pinf = t.GetProperties();
            foreach (PropertyInfo p in pinf)
            {
                if (p.CanRead && p.CanWrite && p.Name != "Params")
                {
                    object pvalue = p.GetValue(st, null);
                    if (pvalue is Expression)
                    {
                        pvalue = Clone(pvalue as Expression);
                    }
                    else if (pvalue is Statement)
                    {
                        pvalue = Clone(pvalue as Statement);
                    }
                    else if (pvalue is BlockStatement)
                    {
                        pvalue = Clone(pvalue as BlockStatement);
                    }
                    p.SetValue(d, pvalue, null);
                }
            }
            foreach (Parameter p in st.Params)
            {
                d.Params.Add(Clone(p));
            }
            return d;
        }
        public static Parameter Clone(this Parameter pa)
        {
            Type t = pa.GetType();
            Parameter d = Activator.CreateInstance(t) as Parameter;
            PropertyInfo[] pinf = t.GetProperties();
            foreach (PropertyInfo p in pinf)
            {
                if (p.CanRead && p.CanWrite)
                {
                    p.SetValue(d, p.GetValue(pa, null), null);
                }
            }
            return d;
        }
        
    }
}
