using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class DelegateFunction
    {
        //
        // Summary:
        //     Gets the method represented by the delegate.
        //
        // Returns:
        //     A System.Reflection.MethodInfo describing the method represented by the delegate.
        //
        // Exceptions:
        //   T:System.MemberAccessException:
        //     The caller does not have access to the method represented by the delegate (for
        //     example, if the method is private).
        public MethodInfo Method { get; }
        //
        // Summary:
        //     Gets the class instance on which the current delegate invokes the instance method.
        //
        // Returns:
        //     The object on which the current delegate invokes the instance method, if the
        //     delegate represents an instance method; null if the delegate represents a static
        //     method.
        public object Target { get; }
        DelegateFunction(object target, MethodInfo method)
        {
            Target = target;
            Method = method;
        }
        public static DelegateFunction CreateDelegate(Type type, string method)
        {
            MethodInfo m = type.GetMethod(method);

            if (m != null)
                return new DelegateFunction(null, m);
            return null;
        }
        public static DelegateFunction CreateDelegate(object target, string method)
        {
            MethodInfo m = target.GetType().GetMethod(method);

            if(m!=null)
                return new DelegateFunction(target, m);
            return null;
        }
        public static DelegateFunction CreateDelegate(object target, string method, bool ignoreCase = false)
        {
            MethodInfo[] ms = target.GetType().GetMethods();
            foreach (var m in ms)
            {
                if (ignoreCase)
                {
                    if(m.Name.Equals(method, StringComparison.OrdinalIgnoreCase))
                        return new DelegateFunction(target, m);
                }
                else
                {
                    if (m.Name.Equals(method))
                        return new DelegateFunction(target, m);
                }
            }
            return null;
        }
        public object Invoke(params object[] parameters)
        {
            return Method.Invoke(Target, parameters);
        }
    }
}
