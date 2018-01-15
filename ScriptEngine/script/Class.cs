using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ScratchNet
{
    public interface Class
    {
        string Name { get; }
        List<Variable> Variables { get; }
        List<Function> Functions { get; }
        List<EventHandler> Handlers { get; }
        List<Expression> Expressions { get; }
        List<BlockStatement> BlockStatements { get; }
        Dictionary<object, Point> Positions { get; }
    }
}
