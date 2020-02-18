using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class StartEventHandler : EventHandlerDelcaration
    {
        public override bool IsProcessEvent(Event e)
        {
            if (e is StartEvent)
                return true;
            return false;
        }
        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "When "));
                desc.Add(new ImageItemDescriptor(this, @"\images\Start-icon.png"));
                desc.Add(new TextItemDescriptor(this, " Clicked"));
                return desc;
            }
        }

    }
}
