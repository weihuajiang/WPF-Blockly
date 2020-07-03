using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class LinkedListEx
    {
        public LinkedListEx Previous { get; set; } = null;
        public LinkedListEx Next { get; set; } = null;
        public object Value { get; set; } = null;

        public override string ToString()
        {
            return "linked list";
        }

        public LinkedListEx() { }
        public LinkedListEx(object v)
        {
            Value = v;
        }
    }
}
