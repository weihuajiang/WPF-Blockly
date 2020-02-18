using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    //用于定义如何执行
    public interface Execution
    {
        Completion Execute(ExecutionEnvironment enviroment);
    }
}
