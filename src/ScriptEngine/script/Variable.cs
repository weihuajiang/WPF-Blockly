using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public abstract class Variable : Declaration
    {
        public abstract string Name { get; set; }
        public abstract string Type { get; set; }
        public abstract object Value { get; set; }
    }
}
