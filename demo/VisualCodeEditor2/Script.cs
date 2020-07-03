using ScratchNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{

    public class Script : Class
    {
        public Script()
        {
            Positions = new Dictionary<object, System.Windows.Point>();
            Variables = new List<Variable>();
            Functions = new List<Function>();
            Handlers = new List<ScratchNet.EventHandler>();
            Expressions = new List<Expression>();
            BlockStatements = new List<BlockStatement>();
        }
        public Dictionary<object, System.Windows.Point> Positions
        {
            get;
            set;
        }
        public List<Variable> Variables
        {
            get;
            set;
        }

        public List<Function> Functions
        {
            get;
            set;
        }

        public List<ScratchNet.EventHandler> Handlers
        {
            get;
            set;
        }
        public List<ScratchNet.Expression> Expressions
        {
            get;
            set;
        }
        public List<BlockStatement> BlockStatements
        {
            get;
            set;
        }
        public List<string> Imports { get; set; } = new List<string>();
        public string Name => "Script";
    }
}
