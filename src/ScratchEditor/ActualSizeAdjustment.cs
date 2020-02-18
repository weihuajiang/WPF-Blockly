using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public interface ActualSizeAdjustment
    {
        void GetActualSize(out double width, out double height);
    }
}
