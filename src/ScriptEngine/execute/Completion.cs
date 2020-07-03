using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public enum CompletionType
    {
        Value,
        Continue,
        Break,
        Return,
        Exception
    }
    public class Completion
    {
        public static Completion Void = new Completion();
        public static Completion Exception(object value, Node location)
        {
            return new Completion(value, CompletionType.Exception) { Location = location };
        }
        public Completion()
        {
            ReturnValue = null;
        }
        public Completion(object value)
        {
            ReturnValue = value;
        }
        public Completion(object value, CompletionType type)
        {
            ReturnValue = value;
            Type = type;
        }
        public Completion(object value, CompletionType type, Node location)
        {
            ReturnValue = value;
            Type = type;
            Location = location;
        }
        public Node Location { get; internal set; } = null;
        public CompletionType Type { get; internal set; } = CompletionType.Value;
        public object ReturnValue { get; internal set; } = null;

        public bool IsValue
        {
            get
            {
                return Type == CompletionType.Value;
            }
        }
        public bool IsException
        {
            get
            {
                return Type == CompletionType.Exception;
            }
        }

    }
}
