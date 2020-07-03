using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ScratchNet
{

    public class WaitStatement : Statement
    {
        public Expression Duration { get; set; }
        public override string ReturnType
        {
            get { return "void"; }
        }
        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            var c = Duration.Execute(enviroment);
            if (c.Type != CompletionType.Value)
                return c;
            try
            {
                float t = TypeConverters.GetValue<float>(c.ReturnValue);
                Thread.Sleep((int)(t * 1000));
            }catch(Exception e)
            {
                return new Completion(e.Message, CompletionType.Exception);
            }
            return Completion.Void;
        }

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "sleep("));
                desc.Add(new ExpressionDescriptor(this, "Duration", "number"));
                desc.Add(new TextItemDescriptor(this, ");"));
                return desc;
            }
        }
        public override string Type
        {
            get
            {
                return "WaitExpression";
            }
        }


        public override BlockDescriptor BlockDescriptor
        {
            get { return null; }
        }
        public override bool IsClosing
        {
            get { return false; }
        }
    }
}
