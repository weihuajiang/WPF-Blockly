using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public interface EventHandler : Function
    {
        bool IsProcessEvent(Event e);
        Event Event { get; set; }
    }
    public class Event
    {

    }
}
