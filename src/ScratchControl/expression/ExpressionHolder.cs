using ScratchNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchControl
{
    public interface ExpressionHolder
    {
        ExpressionDescriptor Descriptor { get; set; }
        void SetExpression(ScratchNet.Expression exp);
    }
}
