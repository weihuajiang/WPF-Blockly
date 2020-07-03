using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class ExecutionEnterEventArgs : EventArgs
    {
        public Node Location { get; internal set; }
        public ExecutionEnterEventArgs(Node node)
        {
            this.Location = node;
        }
    }
    public class ExecutionLeaveEventArgs : EventArgs
    {
        public Node Location { get; internal set; }
        public Completion Value { get; internal set; }
        public ExecutionLeaveEventArgs(Node node, Completion value)
        {
            this.Location = node;
            Value = value;
        }
    }
}
