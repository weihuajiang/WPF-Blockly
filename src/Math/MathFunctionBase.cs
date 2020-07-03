using ScratchNet.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public abstract class MathFunctionBase : Expression
    {
        public override string ReturnType => "number";

        public List<Expression> Args { get; set; } = new List<Expression>();
        public string MathFunction { get;  set; }
        public string FunctionDisplay { get; set; }
        public override Descriptor Descriptor
        {
            get
            {

                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, FunctionDisplay + "("));
                if (Args != null && Args.Count > 0)
                {
                    int i = 0;
                    foreach (Expression p in Args)
                    {
                        if (i != 0)
                            desc.Add(new TextItemDescriptor(this, ", "));
                        string t = "number";
                        desc.Add(new ArgumentDescriptor(this, i, "Args", t));
                        i++;
                    }
                }
                desc.Add(new TextItemDescriptor(this, ")"));
                return desc;
            }
        }

        public override string Type => "MathExpression";
        Completion CallMathFunc<T>(List<object> values)
        {
            try
            {
                var ps = new List<T>();
                foreach (var v in values)
                {
                    ps.Add(TypeConverters.GetValue<T>(v));
                }
                var methods = typeof(Math).GetMethods();
                foreach (var m in methods)
                {
                    if (m.Name.Equals(MathFunction))
                    {
                        var pa = m.GetParameters();
                        if (ps.Count != pa.Length)
                            continue;
                        bool isFunc = true;
                        foreach (var p in pa)
                        {
                            if (!p.ParameterType.Equals(typeof(T)))
                                isFunc=false;
                            break;
                        }
                        if (isFunc)
                        {
                            object[] mps = new object[ps.Count];
                            for (int i = 0; i < ps.Count; i++)
                                mps[i] = ps[i];
                            return new Completion(m.Invoke(null, mps));
                        }
                    }
                }
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return Completion.Exception(e.Message, this);
            }
            return Completion.Exception("no function " + MathFunction + " was not found", this);
        }
        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            try
            {
                List<object> values = new List<object>();
                for (int i = 0; i < Args.Count; i++)
                {
                    if (Args[i] == null)
                        return Completion.Exception(Language.NullException, this);
                    var c = Args[i].Execute(enviroment);
                    if (!c.IsValue)
                        return c;
                    if (!TypeConverters.IsNumber(c.ReturnValue))
                        return Completion.Exception(Language.NotNumberException, Args[i]);
                    values.Add(c.ReturnValue);
                }
                int methodCount=0;
                var ms = typeof(Math).GetMethods();
                MethodInfo doubleMethod = null;
                foreach(var m in ms)
                {
                    if (m.Name.Equals(MathFunction) && m.GetParameters().Length == Args.Count)
                    {
                        methodCount++;
                        bool isDouble = true;
                        foreach(var p in m.GetParameters())
                        {
                            if (!p.ParameterType.Equals(typeof(double)))
                            {
                                isDouble = false;
                                break;
                            }
                        }
                        if (isDouble)
                            doubleMethod = m;
                    }
                }
                if (methodCount < 0)
                {
                    return Completion.Exception("no function found", this);
                }
                else if (methodCount == 1)
                {
                    if (doubleMethod != null)
                    {
                        var ps = doubleMethod.GetParameters();
                        List<object> fparam = new List<object>();
                        for (int i = 0; i < Args.Count; i++)
                            fparam.Add(TypeConverters.GetValue(values[i], ps[i].ParameterType));
                        return new Completion(doubleMethod.Invoke(null, fparam.ToArray()));
                    }
                    else
                    {
                        MethodInfo method = typeof(Math).GetMethod(MathFunction);
                        var ps = method.GetParameters();
                        List<object> fparam = new List<object>();
                        for (int i = 0; i < Args.Count; i++)
                            fparam.Add(TypeConverters.GetValue(values[i], ps[i].ParameterType));
                        return new Completion(method.Invoke(null, fparam.ToArray()));
                    }
                }
                else if (methodCount == 2)
                {
                    if (doubleMethod != null)
                    {
                        var ps = doubleMethod.GetParameters();
                        List<object> fparam = new List<object>();
                        for (int i = 0; i < Args.Count; i++)
                            fparam.Add(TypeConverters.GetValue(values[i], ps[i].ParameterType));
                        return new Completion(doubleMethod.Invoke(null, fparam.ToArray()));
                    }
                    return Completion.Exception(Language.UnknowException, this);
                }
                else
                {
                    var T = TypeConverters.GetMaxTypes(values.ToArray());
                    if (T.Equals(typeof(char)))
                    {
                        return CallMathFunc<int>(values);
                    }
                    else if (T.Equals(typeof(int)))
                    {
                        return CallMathFunc<int>(values);
                    }
                    else if (T.Equals(typeof(float)))
                        return CallMathFunc<float>(values);
                    else
                        return CallMathFunc<double>(values);
                }
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return Completion.Exception(e.Message, this);
            }
        }
    }
}
