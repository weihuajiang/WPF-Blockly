using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class MessageEvent : Event
    {
        public string Message { get; internal set; }
        public MessageEvent(string msg)
        {
            Message = msg;
        }
    }
}
