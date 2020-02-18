using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ScratchControl
{
    class GlobalResource
    {
        public static Brush SelectionBorderBrush = new SolidColorBrush(Colors.Orange);
        public static Brush HoverBorderBrush = new SolidColorBrush(Colors.Orange);
        public static Brush CanDropBorderBrush = new SolidColorBrush(Colors.Green);
        public static Brush CanNotDropBorderBrush = new SolidColorBrush(Colors.Red);
    }
}
