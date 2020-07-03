using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class SpeakVolumeExpression : Expression, IAssignment
    {
        public override string ReturnType => "number";

        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "volume"));
                return desc;
            }
        }

        public override string Type => "SpeakVolumeExpression";

        public Completion Assign(ExecutionEnvironment environment, object value)
        {
            if (!TypeConverters.IsNumber(value))
                return Completion.Exception(Properties.Language.NotNumber, this);
            int v = (int)value;
            if (v < 0 || v > 100)
                return Completion.Exception(Properties.Language.VolumeOutOfRange, this);
            SpeachBase.GetSpeech(environment).Volume = v;
            return new Completion(v);
        }

        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            return new Completion(SpeachBase.GetSpeech(enviroment).Volume);
        }
    }
}
