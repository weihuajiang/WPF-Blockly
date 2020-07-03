using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class ExecutionAbortException : Exception
    {
        public ExecutionAbortException() : base()
        {

        }
        public ExecutionAbortException(string message) : base(message)
        {

        }
    }
}
