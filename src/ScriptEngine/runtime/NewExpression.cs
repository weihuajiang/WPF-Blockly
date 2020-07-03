using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class NewExpression : Expression
    {
        public Type ObjectType { get; set; }

        public List<Expression> Parameters { get; set; } = new List<Expression>();
        public string Name { get; set; }

        public override string Type => "NewExpression";

        public override string ReturnType => "any";

        public override Descriptor Descriptor
        {
            get
            {

                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "new", true));
                desc.Add(new TextItemDescriptor(this, " "+Name + "("));
                    if (Parameters != null && Parameters.Count > 0)
                    {
                        int i = 0;
                        foreach (Expression p in Parameters)
                        {
                            if (i != 0)
                                desc.Add(new TextItemDescriptor(this, ", "));
                            desc.Add(new ArgumentDescriptor(this, i, "Parameters", "any"));
                            i++;
                        }
                    }
                    desc.Add(new TextItemDescriptor(this, ")"));
                return desc;
            }
        }

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            try
            {
                foreach (var c in ObjectType.GetConstructors())
                {
                    if ((Parameters.Count > 0 && c.GetParameters().Length == Parameters.Count) ||
                        (Parameters.Count == 0 && (c.GetParameters() == null || c.GetParameters().Length == 0)))
                    {
                        object[] ps = new object[Parameters.Count];
                        for (int i = 0; i < Parameters.Count; i++)
                        {
                            if (Parameters[i] == null)
                                return Completion.Exception(Properties.Language.ParameterNullException, this);
                            try
                            {
                                var p=Parameters[i].Execute(enviroment);
                                if (p.Type != CompletionType.Value)
                                    return p;
                                ps[i] = p.ReturnValue;
                            }
                            catch (Exception e)
                            {
                                return Completion.Exception(e.Message, Parameters[i]);
                            }
                        }
                        object value = c.Invoke(ps);
                        return new Completion(value);
                    }
                }
            }catch(Exception ex)
            {
                return Completion.Exception(ex.Message, this);
            }
            return Completion.Exception(Properties.Language.FailedNewException, this);
        }
    }
}
