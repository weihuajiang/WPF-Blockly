using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class CallStatement : Statement
    {
        public string Function
        {
            get;
            set;
        }
        public string FunctionNameFormat { get; set; }
        public CallStatement()
        {
            Args = new List<Expression>();
            ArgTyps = new List<string>();
        }
        public List<Expression> Args { get;set; }
        public List<string> ArgTyps { get; set; }
        public string ReturnType
        {
            get { return "number|boolean|string"; }
        }

        public Completion Execute(ExecutionEnvironment enviroment)
        {
            return Completion.Void;
        }

        public Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                if (string.IsNullOrEmpty(FunctionNameFormat))
                {
                    desc.Add(new TextItemDescriptor(this, Function + "("));
                    if (Args != null && Args.Count > 0)
                    {
                        int i = 0;
                        foreach (Expression p in Args)
                        {
                            if (i != 0)
                                desc.Add(new TextItemDescriptor(this, ", "));
                            string t = "";
                            if (ArgTyps != null && ArgTyps.Count >= i + 1)
                                t = ArgTyps[i];
                            desc.Add(new ArgumentDescriptor(this, i, "Args", t));
                            i++;
                        }
                    }
                    desc.Add(new TextItemDescriptor(this, ")"));
                }
                else
                {
                    string[] part = FunctionNameFormat.Split(new string[] { "[[", "]]" }, StringSplitOptions.RemoveEmptyEntries);
                    int i = 0;
                    foreach (string s in part)
                    {
                        if (s.StartsWith("{{") && s.EndsWith("}}"))
                        {
                            int length = s.Length;
                            int index = int.Parse(s.Substring(2).Substring(0, length - 4));
                            string t = "";
                            if (ArgTyps != null && ArgTyps.Count >= i + 1)
                                t = ArgTyps[i];
                            desc.Add(new ArgumentDescriptor(this, i, "Args", t));
                            i++;
                        }
                        else
                            desc.Add(new TextItemDescriptor(this, s));
                    }
                }
                return desc;
            }
        }

        public string Type
        {
            get { return ""; }
        }

        public BlockDescriptor BlockDescriptor
        {
            get { return null; }
        }
        public bool IsClosing
        {
            get { return false; }
        }
        //execution 
        int current = 0;
        List<object> pVals;
        object result;

        public ExecutionEnvironment StartCall(ExecutionEnvironment e)
        {
            current = 0;
            pVals = new List<object>();
            result = null;
            return e;
        }

        public Completion EndCall(ExecutionEnvironment e)
        {
            return new Completion(result);
        }

        public bool PopStack(out object execution, out ExecutionCallback callback, ExecutionEnvironment _env)
        {
            callback = Callback;
            if (Args != null && current < Args.Count)
            {
                execution = Args[current];
                return true;
            }
            else
            {
                Function func = null;
                Instance This = _env.This;
                foreach (Function f in This.Class.Functions)
                {
                    if (f.Name == Function)
                        func = f;
                }
                int i = 0;
                foreach (object v in pVals)
                {
                    func.Params[i].Value = v;
                    i++;
                }
                execution = func;
                return false;
            }
        }
        Nullable<DateTime> Callback(object value, object exception, ExecutionEnvironment e)
        {
            if (Args != null && current < Args.Count)
                pVals.Add( value);
            else
                result = value;
            current++;
            return null;
        }
        public bool HandleException(object exception)
        {
            throw new NotImplementedException();
        }
    }
}
