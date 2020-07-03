using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public abstract class EventHandler : Function
    {
        public abstract bool IsProcessEvent(Event e);
        public abstract Event Event { get; set; }
    }
    public class Event
    {

    }
}
