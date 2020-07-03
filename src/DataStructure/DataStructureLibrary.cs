using ScratchNet.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class DataStructureLibrary : Library
    {
        public DataStructureLibrary()
        {
            Name = "Data Structure";
            Title = Language.Title;
            DefaultColor = "orange";
            Description = Language.Description;
            CommandGroup stack = new CommandGroup("binary tree", "");
            stack.Add(new Command("new", "create a new binary tree", true, new NewBinaryTreeExpression()));
            stack.Add(new Command("value", "value of binary tree", true, new BinaryTreeValueExpression()));
            stack.Add(new Command("left", "left node of binary tree", true, new BinaryTreeLeftExpression()));
            stack.Add(new Command("right", "right node of binary tree", true, new BinaryTreeRightExpression()));
            stack.Add(new Command("null", "null value", true, new NullExpression()));
            Add(stack);
            CommandGroup link = new CommandGroup("linked list", "");
            link.Add(new Command("new", "create a new linked list", true, new NewLinkedListExpression()));
            link.Add(new Command("value", "value of linked list", true, new LinkedListValueExpression()));
            link.Add(new Command("next", "next node of binary tree", true, new LinkedListNextExpression()));
            link.Add(new Command("previous", "previous node of binary tree", true, new LinkedListPreviousExpression()));
            link.Add(new Command("null", "null value", true, new NullExpression()));
            Add(link);
        }
    }
}
