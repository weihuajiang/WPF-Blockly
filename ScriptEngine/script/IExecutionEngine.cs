using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public interface IExecutionEngine
    {
        void SendEvent(Event e);
        Instance CreateInstance(Class c);
        ExecutionEnvironment BaseEnvironment { get; }
    }
}
