using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class FunctionDeclarationEx : Function
    {
        public FunctionDeclarationEx()
        {
            Params = new List<Parameter>();
            Body = new BlockStatement();
        }
        public string Format { get; set; }
        public List<Parameter> Params { get; set; }
        public string Name { get; set; }
        public BlockStatement Body { get; set; }

        public Completion Execute(ExecutionEnvironment enviroment)
        {
            throw new NotImplementedException();
        }

        public string ReturnType
        {
            get { return "number|boolean|string"; }
        }

        public Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                if (!string.IsNullOrEmpty(Format))
                {
                    string[] part = Format.Split(new string[] { "[[", "]]" }, StringSplitOptions.RemoveEmptyEntries);
                    int i = 0;
                    foreach (string s in part)
                    {
                        if (s.StartsWith("{{") && s.EndsWith("}}"))
                        {
                            int length = s.Length;
                            int index = int.Parse(s.Substring(2).Substring(0, length - 4));
                            Parameter p = Params[index];
                            desc.Add(new ParameterDescriptor(this, i, p.Name, p.Type, p.Direction));
                            i++;
                        }
                        else
                            desc.Add(new TextItemDescriptor(this, s));
                    }
                }
                else
                {
                    desc.Add(new TextItemDescriptor(this, Name + "("));
                    int i = 0;
                    foreach (Parameter p in Params)
                    {
                        if (i != 0)
                            desc.Add(new TextItemDescriptor(this, ","));
                        desc.Add(new ParameterDescriptor(this, i, p.Name, p.Type, p.Direction));
                        i++;
                    }
                    desc.Add(new TextItemDescriptor(this, ")"));
                }
                return desc;
            }
        }

        public string Type
        {
            get { return "FunctionDeclarationEx"; }
        }


        public BlockDescriptor BlockDescriptor
        {
            get
            {
                BlockDescriptor desc = new BlockDescriptor();
                desc.Add(new BlockStatementDescriptor(this, "Body"));
                return desc;
            }
        }
        public bool IsClosing
        {
            get { return false; }
        }

        public ExecutionEnvironment StartCall(ExecutionEnvironment e)
        {
            ExecutionEnvironment _env = new ExecutionEnvironment(new ExecutionEnvironment(e.Engine.BaseEnvironment, e.This));
            if (Params != null)
            {
                foreach (Parameter p in Params)
                {
                    _env.RegisterValue(p.Name, p.Value);
                }
            }
            return _env;
        }

        public Completion EndCall(ExecutionEnvironment e)
        {
            return Completion.Void;
        }

        public bool PopStack(out object execution, out ExecutionCallback callback, ExecutionEnvironment e)
        {
            execution = Body;
            callback = Callback;
            return false;
        }
        Nullable<DateTime> Callback(object value, object exception, ExecutionEnvironment e)
        {
            return null;
        }
        public bool HandleException(object exception)
        {
            throw new NotImplementedException();
        }
    }
}
