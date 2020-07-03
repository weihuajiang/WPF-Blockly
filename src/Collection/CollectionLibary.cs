using ScratchNet.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class CollectionLibary : Library
    {
        public CollectionLibary()
        {
            Name = "Collection";
            Title = Language.Title;
            DefaultColor = "orange";
            Description = Language.Description;
            CommandGroup stack = new CommandGroup("stack", "stack data structure");
            stack.Add(new Command("new", "create a new stack object", true, new NewStackExpression()));
            stack.Add(new Command("push", "push element to stack", true, new PushStackExpression()));
            stack.Add(new Command("pop", "pop element from stack", true, new PopStackExpression()));
            stack.Add(new Command("peek", "peek the top element in stack", true, new PeekExpression()));
            stack.Add(new Command("count", "get size of", true, new CountExpression()));
            stack.Add(new Command("clear", "clear all the element in stack", true, new ClearExpression()));
            Add(stack);
            CommandGroup queue = new CommandGroup("queue", "queue data structure");
            queue.Add(new Command("new", "create a new queue", true, new NewQueueExpression()));
            queue.Add(new Command("enqueue", "put element to queue", true, new EnqueueExpression()));
            queue.Add(new Command("dequeue", "get element from queue", true, new DequeueExpression()));
            queue.Add(new Command("peek", "peek the top element in queue", true, new PeekExpression()));
            queue.Add(new Command("count", "get size of queue", true, new CountExpression()));
            queue.Add(new Command("clear", "clear all the element in queue", true, new ClearExpression()));
            Add(queue);
            CommandGroup list = new CommandGroup("list", "list data structure");
            list.Add(new Command("new", "create a new list", true, new NewListExpression()));
            list.Add(new Command("add", "add element to list", true, new ListAddExpression()));
            list.Add(new Command("value", "get and set value in list", true, new ListValueExpression()));
            list.Add(new Command("insert", "insert element into list in given position", true, new ListInsertExpression()));
            list.Add(new Command("remove", "remove an element", true, new ListRemoveExpression()));
            list.Add(new Command("removeAt", "remove element at position", true, new ListRemoveAtExpression()));
            list.Add(new Command("count", "get size of list", true, new CountExpression()));
            list.Add(new Command("clear", "clear all the element in list", true, new ClearExpression()));
            Add(list);
            CommandGroup dictionary = new CommandGroup("dictionary", "dictionary data structure");
            dictionary.Add(new Command("new", "create a new dictionary", true, new NewDictionaryExpression()));
            dictionary.Add(new Command("add", "add element to dictionary", true, new DictionaryAddExpression()));
            dictionary.Add(new Command("remove", "remove an element from dictionary by key", true, new ListRemoveExpression()));
            dictionary.Add(new Command("value", "get and set value in dictionary", true, new ListValueExpression()));
            dictionary.Add(new Command("count", "get size of list", true, new CountExpression()));
            dictionary.Add(new Command("clear", "clear all the element in list", true, new ClearExpression()));
            Add(dictionary);
        }
    }
}
