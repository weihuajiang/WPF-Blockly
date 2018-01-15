using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public interface Function : Declaration, Execution2
    {
        string ReturnType { get; }

        //描述步骤组成，用来绘制
        Descriptor Descriptor { get; }

        //ID used to store and load
        string Type { get; }

        //parameter descriptions
        string Name { get; set; }
        List<Parameter> Params { get; set; }
        string Format { get;set;}
        BlockStatement Body { get; set; }
        BlockDescriptor BlockDescriptor { get; }
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