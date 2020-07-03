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
        public static Type GetMaxTypes( params object[] values)
        {
            int max = 0;
            foreach(var v in values)
            {
                var vc = (int)(v as IConvertible).GetTypeCode();
                if (vc > max)
                    max = vc;
            }
            var t = (TypeCode)max;
            switch (t)
            {
                case TypeCode.Char:
                    return typeof(char);
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
                case TypeCode.DateTime:
                    return typeof(DateTime);
                case TypeCode.String:
                    return typeof(string);
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
                case TypeCode.Char:
                case TypeCode.Byte:
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
            throw new NotSupportedException(string.Format(Properties.Language.CanNotConvertException, value.GetType(), T));
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
                {
                    try
                    {
                        return (T)tc2.ConvertTo(value, typeof(T));
                    }
                    catch { }
                }
                else
                    try
                    {
                        return (T)tc.ConvertFromString(tc2.ConvertToString(value));
                    }
                    catch { }
            }
            try
            {
                return (T)value;
            }
            catch { }
            try
            {
                if (typeof(T).Equals(typeof(int)))
                    return (T)(object)Convert.ToInt32(value);
            }
            catch { }
            try
            {
                if (typeof(T).Equals(typeof(float)))
                    return (T)(object)Convert.ToSingle(value);
            }
            catch { }
            try
            {
                if (typeof(T).Equals(typeof(double)))
                    return (T)(object)Convert.ToDouble(value);
            }
            catch { }
            try
            {
                if (typeof(T).Equals(typeof(bool)))
                    return (T)(object)Convert.ToBoolean(value);
            }
            catch { }
            try
            {
                if (typeof(T).Equals(typeof(long)))
                    return (T)(object)Convert.ToInt64(value);
            }
            catch { }
            try
            {
                if (typeof(T).Equals(typeof(char)))
                    return (T)(object)Convert.ToChar(value);
            }
            catch { }
            throw new NotSupportedException(string.Format(Properties.Language.CanNotConvertException, value.GetType(), typeof(T)));

        }
    }
}
