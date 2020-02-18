using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public interface Variable : Declaration
    {
        string Name { get; set; }
        string Type { get; set; }
        object Value { get; set; }
    }
}
