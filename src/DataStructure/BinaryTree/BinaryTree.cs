using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class BinaryTree
    {
        public BinaryTree Left { get; set; } = null;
        public BinaryTree Right { get; set; } = null;
        public object Value { get; set; }
        public BinaryTree() { }
        public BinaryTree(object value)
        {
            Value = value;
        }
        public override string ToString()
        {
            return "binary tree";
        }
    }
}
