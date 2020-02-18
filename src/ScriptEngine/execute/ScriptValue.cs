using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class ScriptValue
    {
        public Type Type { get; internal set; }
        object value;
        public object Value
        {
            get
            {
                return value;
            }
        }
        public T GetValue<T>()
        {
            TypeConverter tc = TypeDescriptor.GetConverter(typeof(T));
            if (tc.CanConvertFrom(Type))
                return (T)tc.ConvertFrom(value);
            else
            {
                TypeConverter tc2 = TypeDescriptor.GetConverter(Type);
                if (tc.CanConvertTo(typeof(T)))
                    return (T)tc2.ConvertTo(value, typeof(T));
                else
                    try
                    {
                        return (T)tc.ConvertFromString(tc2.ConvertToString(value));
                    }
                    catch { }
            }
            throw new NotSupportedException(string.Format("can not convert {0} to {1}", Type, typeof(T)));

        }
        public object GetArrayValue(int index)
        {
            return GetArrayValue<object>(index);
        }
        public T GetArrayValue<T>(int index)
        {
            if (value is IList)
            {
                var array = value as IList;
                if (array != null)
                {
                    if (array.Count > index && index >= 0)
                        return (T)array[index];
                    else
                        throw new IndexOutOfRangeException();
                }
            }
            throw new NotSupportedException("object in not a array");
        }
        ScriptValue(object v, Type type)
        {
            value = v;
            Type = type;
        }
        public static ScriptValue New<T>(T v) 
        {
            return new ScriptValue(v, typeof(T));
        }
    }
}
