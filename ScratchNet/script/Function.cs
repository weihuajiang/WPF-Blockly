using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public interface Function : Declaration
    {
        //parameter descriptions
        string Name { get; set; }
        List<Parameter> Params { get; set; }
        BlockStatement Body { get; set; }
        BlockDescriptor BlockDescriptor { get; }
    }
    public class Parameter
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public ParamDirection Direction { get; set; }
    }
    public enum ParamDirection
    {
        In,
        Out,
        Ref
    }
}