using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class StartEvent: Event
    {
    }
    public class MouseEvent : Event
    {

    }
    public class KeyEvent : Event
    {
        public int Key { get; set; }
    }
    public class ClickEvent : Event
    {

    }
}
