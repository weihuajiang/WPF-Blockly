using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class ClickEventHandler : EventHandlerDelcaration
    {
        public override bool IsProcessEvent(Event e)
        {
            if (e is ClickEvent)
                return true;
            return false;
        }
        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "When Charater Clicked"));
                return desc;
            }
        }
    }
}
