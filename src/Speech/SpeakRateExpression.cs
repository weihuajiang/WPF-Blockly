using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class SpeakRateExpression : Expression, IAssignment
    {
        public override string ReturnType => "number";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "rate"));
                return desc;
            }
        }

        public override string Type => "SpeaRateExpression";

        public Completion Assign(ExecutionEnvironment environment, object value)
        {
            if (!TypeConverters.IsNumber(value))
                return Completion.Exception(Properties.Language.NotNumber, this);
            int v = (int)value;
            if (v < -10 || v > 10)
                return Completion.Exception(Properties.Language.RateOutOfRange, this);
            SpeachBase.GetSpeech(environment).Rate = v;
            return new Completion(v);
        }

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            return new Completion(SpeachBase.GetSpeech(enviroment).Rate);
        }
    }
}
