using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{

    //定义代码块和flow control， 比如if, while
    public class BlockStatement : Node
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
        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Body == null || Body.Count == 0)
                return Completion.Void;
            Completion c = Completion.Void;
            for (int i = 0; i < Body.Count; i++)
            {
                Statement stat = Body[i];
                c = stat.Execute(enviroment);
                if(c.Type!= CompletionType.Value)
                {
                    return c;
                }
            }
            return c;
        }
    }
}
