using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class KeyEventHandler : EventHandlerDelcaration
    {
        public int Key { get; set; }
        public override bool IsProcessEvent(Event e)
        {
            if (e is KeyEvent)
            {
                KeyEvent k = e as KeyEvent;
                if (Key == k.Key)
                    return true;
            }
            return false;
        }
        public override Descriptor Descriptor
        {
            get
            {
                Descriptor desc = new Descriptor();
                desc.Add(new TextItemDescriptor(this, "When Press "));
                object[] texts = new object[5 + 10 + 26]; 
                object[] values = new object[5+10+26];
                texts[0] = "Blank";
                values[0] = (int)System.Windows.Input.Key.Space;
                texts[1] = "Up";
                values[1] = (int)System.Windows.Input.Key.Up;
                texts[2] = "Down";
                values[2] = (int)System.Windows.Input.Key.Down;
                texts[3] = "Left";
                values[3] = (int)System.Windows.Input.Key.Left;
                texts[4] = "Right";
                values[4] = (int)System.Windows.Input.Key.Right;
                for (int i = 0; i < 10; i++)
                {
                    values[5 + i] = 34 + i;
                    texts[5 + i] = (char)((int)'0' + i)+"";
                }
                for (int i = 0; i < 26; i++)
                {
                    values[15 + i] = 44 + i;
                    texts[15 + i] = (char)((int)'A' + i) + "";
                }

                desc.Add(new SelectionItemDescriptor(this, "Key", texts, values));
                desc.Add(new TextItemDescriptor(this, " Key"));
                return desc;
            }
        }
    }
}
