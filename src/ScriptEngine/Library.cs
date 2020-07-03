using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class Library : List<CommandGroup>
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DefaultColor { get; set; }
    }
    public class CommandGroup : List<Command> { 
        public CommandGroup()
        {

        }
        public CommandGroup(string name, string desc)
        {
            Name = name;
            Description = desc;
        }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class Command
    {
        public Command()
        {

        }
        public Command(string name, string desc, bool isColorEditable, Node node)
        {
            Name = name;
            Description = desc;
            IsColorEditable = isColorEditable;
            Step = node;
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsColorEditable { get; set; }
        public Node Step { get; set; }
    }
}
