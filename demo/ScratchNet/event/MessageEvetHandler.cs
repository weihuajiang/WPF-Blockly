using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class MessageEvetHandler : EventHandlerDelcaration
    {
        public string Message { get; set; }
        public override bool IsProcessEvent(Event e)
        {
            if (e is MessageEvent && (e as MessageEvent).Message == Message)
            {
                return true;
            }
            return false;
        }
        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "When Recieve Message "));

                desc.Add(new SelectionItemDescriptor(this, "Message", CurrentEnviroment.Messages, CurrentEnviroment.Messages));
                return desc;
            }
        }
    }
}
