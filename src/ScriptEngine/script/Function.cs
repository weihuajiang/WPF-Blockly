using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public abstract class Function : Declaration
    {
        public abstract string ReturnType { get; }

        //描述步骤组成，用来绘制
        public abstract Descriptor Descriptor { get; }

        //ID used to store and load
        public abstract string Type { get; }

        //parameter descriptions
        public abstract string Name { get; set; }
        public abstract List<Parameter> Params { get; set; }
        public abstract string Format { get;set;}
        public abstract BlockStatement Body { get; set; }
        public abstract BlockDescriptor BlockDescriptor { get; }
    }
    public class Parameter
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public ParamDirection Direction { get; set; }
        public object Value { get; set; }
    }
    public enum ParamDirection
    {
        In,
        Out,
        Ref
    }
}