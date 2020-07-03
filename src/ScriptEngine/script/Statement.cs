using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    //定义程序步骤,单步可执行语句
    public abstract class Statement: Node
    {
        public abstract string ReturnType { get; }

        //描述步骤组成，用来绘制
        public abstract Descriptor Descriptor { get; }

        //ID used to store and load
        public abstract string Type { get; }
        public abstract BlockDescriptor BlockDescriptor { get; }
        public abstract bool IsClosing { get; }
    }
    
}
