using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public interface EventListener
    {
        bool CanRun(object eventName, object parameter);
    }
}
