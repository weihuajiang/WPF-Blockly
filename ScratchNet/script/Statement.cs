using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    //定义程序步骤,单步可执行语句
    public interface Statement: Execution
    {
        BlockDescriptor BlockDescriptor { get; }
    }
    //定义代码块和flow control， 比如if, while
    public class BlockStatement
    {
        ObservableCollection<Statement> _body = new ObservableCollection<Statement>();
        public ObservableCollection<Statement> Body
        {
            get { return _body; }
            set
            {
                _body = value;
            }
        }
    }
    
}
