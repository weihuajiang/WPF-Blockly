using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    //定义表达式
    //expression中不能有statement
    public interface Expression : Execution, INode
    {
        string ReturnType { get; }

        //描述步骤组成，用来绘制
        Descriptor Descriptor { get; }

        //ID used to store and load
        string Type { get; }
    }
}
