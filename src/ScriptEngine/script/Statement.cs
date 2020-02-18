using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    //定义程序步骤,单步可执行语句
    public interface Statement: Execution, INode
    {
        string ReturnType { get; }

        //描述步骤组成，用来绘制
        Descriptor Descriptor { get; }

        //ID used to store and load
        string Type { get; }
        BlockDescriptor BlockDescriptor { get; }
        bool IsClosing { get; }
    }
    
}
