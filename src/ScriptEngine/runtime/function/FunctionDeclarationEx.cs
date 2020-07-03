using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class FunctionDeclaration : Function
    {
        public FunctionDeclaration()
        {
            Params = new List<Parameter>();
            Body = new BlockStatement();
        }
        public override string Format { get; set; }
        public override List<Parameter> Params { get; set; }
        /// <summary>
        /// function name
        /// </summary>
        public override string Name { get; set; }
        public override BlockStatement Body { get; set; }
        public bool ShowFunctionName { get; set; } = true;

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Body == null)
                return Completion.Void;
            var c = Body.Execute(enviroment);
            if (c.Type == CompletionType.Value || c.Type == CompletionType.Exception)
                return c;
            if (c.Type == CompletionType.Return)
                return new Completion(c.ReturnValue);
            return Completion.Void;
        }

        public override string ReturnType
        {
            get { return "number|boolean|string"; }
        }

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "function ", true));
                if (!ShowFunctionName && !string.IsNullOrEmpty(Format))
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

        public override string Type
        {
            get { return "FunctionDeclarationEx"; }
        }


        public override BlockDescriptor BlockDescriptor
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

    }
}
