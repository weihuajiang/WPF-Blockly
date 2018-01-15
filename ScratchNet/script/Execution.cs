using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class Completion
    {
        public static Completion Void = new Completion();
        public Completion()
        {

        }
        public Completion(object value)
        {

        }
        public object ReturnValue { get; internal set; }

    }
    //用于定义如何执行
    public interface Execution
    {
        Completion Execute(ExecutionEnvironment enviroment);
        string ReturnType { get; }

        //描述步骤组成，用来绘制
        Descriptor Descriptor { get; }

        //ID used to store and load
        string Type { get; }
    }

    public interface Execution2
    {
        ExecutionEnvironment GetEnvironment(ExecutionEnvironment current);
        void StartCall();
        Completion EndCall();
        bool PopStack(out object execution, out ExecutionCallback callback);
        bool HandleException(object exception);
    }
    public class StackItem
    {
        public Execution Value { get; set; }
    }
    //每步执行的回调函数，value为步骤计算值，exception为异常， DateTime为下个步骤执行时间，null为立刻执行
    public delegate Nullable<DateTime> ExecutionCallback(object value, object exception);
}
