using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ScratchNet
{
    public class TypeColorChangeEventArgs : EventArgs
    {
        public object Type { get; internal set; }
        public Color Color { get; internal set; }
    }
}
