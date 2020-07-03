using ScratchNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ScratchControl
{
    public interface ExpressionControl
    {
        Brush ExpressionBackground { set; }
        Brush ExpressionBorderBrush {  set; }
        Expression Expression { get; set; }

        bool IsSelected {set; }
        bool IsHovered {  set; }
        Nullable<bool> CanDrop { set; }
    }
}
