using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class Instance
    {
        public Instance(Class c)
        {
            Class = c;
            States = new Dictionary<string, object>();
        }
        public Class Class { get; internal set; }
        public Dictionary<string, object> States { get; internal set; }
    }
}
