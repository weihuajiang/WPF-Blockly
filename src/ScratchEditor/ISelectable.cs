using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public interface ISelectable
    {
        bool IsSelected { get; set; }
        Node SelectedValue { get; }
    }
}
