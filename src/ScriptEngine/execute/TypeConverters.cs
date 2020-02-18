using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class TypeConverters
    {
        public static Type GetNumberTypes(object a, object b)
        {
            var ac = (int)(a as IConvertible).GetTypeCode();
            var bc = (int)(b as IConvertible).GetTypeCode();
            var t = (TypeCode)Math.Max(ac, bc);
            switch (t)
            {
                case TypeCode.Int32:
                    return typeof(int);
                case TypeCode.UInt32:
                    return typeof(uint);
                case TypeCode.Single:
                    return typeof(float);
                case TypeCode.Double:
                    return typeof(double);
                case TypeCode.Decimal:
                    return typeof(decimal);
                case TypeCode.Int16:
                    return typeof(short);
                case TypeCode.UInt16:
                    return typeof(ushort);
                case TypeCode.Int64:
                    return typeof(long);
                case TypeCode.UInt64:
                    return typeof(ulong);
                default:
                    return null;
            }
        }
        public static bool IsNumber(object value)
        {
            if (value == null)
                return false;
            var c = value as IConvertible;
            if (c == null)
                return false;
            switch (c.GetTypeCode())
            {
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                case TypeCode.Int16:
                case TypeCode.Int64:
                    return true;
                default:
                    return false;
            }

        }
        public static object GetValue(object value, Type T)
        {
            if (value == null)
            {
                return null;
            }
            if (T.Equals(value.GetType()))
                return value;
            TypeConverter tc = TypeDescriptor.GetConverter(T);
            if (tc.CanConvertFrom(value.GetType()))
                return tc.ConvertFrom(value);
            else
            {
                TypeConverter tc2 = TypeDescriptor.GetConverter(value.GetType());
                if (tc.CanConvertTo(T))
                    return tc2.ConvertTo(value, T);
                else
                    try
                    {
                        return tc.ConvertFromString(tc2.ConvertToString(value));
                    }
                    catch { }
            }
            throw new NotSupportedException(string.Format("can not convert {0} to {1}", value.GetType(), T));
        }
        public static T GetValue<T>(object value)
        {
            if (value == null)
            {
                return default(T);
            }
            if (typeof(T).Equals(value.GetType()))
                return (T)value;
            TypeConverter tc = TypeDescriptor.GetConverter(typeof(T));
            if (tc.CanConvertFrom(value.GetType()))
                return (T)tc.ConvertFrom(value);
            else
            {
                TypeConverter tc2 = TypeDescriptor.GetConverter(value.GetType());
                if (tc.CanConvertTo(typeof(T)))
                    return (T)tc2.ConvertTo(value, typeof(T));
                else
                    try
                    {
                        return (T)tc.ConvertFromString(tc2.ConvertToString(value));
                    }
                    catch { }
            }
            throw new NotSupportedException(string.Format("can not convert {0} to {1}", value.GetType(), typeof(T)));

        }
    }
}
