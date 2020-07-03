using ScratchNet.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class Literal : Expression//, IAssignment
    {
        public Literal() { }
        public Literal(string raw) { Raw = raw; }
        public override string ReturnType
        {
            get { return ""; }
        }
        public static string GetStringLateral(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < text.Length; )
            {
                char c = text[i];
                if (c == '\\')
                {
                    i += 1;
                    char n = text[i];
                    switch (n)
                    {
                        //case 'u':
                        //case 'x':
                        //    break;
                        case 'n':
                            sb.Append("\n");
                            break;
                        case 'r':
                            sb.Append("\r");
                            break;
                        case 't':
                            sb.Append("\t");
                            break;
                        case 'b':
                            sb.Append("\b");
                            break;
                        case 'f':
                            sb.Append("\f");
                            break;
                        case 'v':
                            sb.Append("\x0B");
                            break;
                        case '\\':
                            sb.Append('\\');
                            break;
                        case '\"':
                            sb.Append('\"');
                            break;
                        default:
                            throw new Exception(Language.WrongStringFormat);
                    }
                }
                else if (c == '\"')
                    throw new Exception(Language.WrongStringFormat);
                else
                    sb.Append(c);
                i += 1;
            }
            return sb.ToString();
        }
        protected override Completion ExecuteImpl(ExecutionEnvironment enviroment)
        {
            if (Raw == null)
                return new Completion(null);
            int intValue;
            float floatValue;
            double doubleValue ;
            bool b;
            if (Raw.StartsWith("\"") && Raw.EndsWith("\""))
            {
                string str = GetStringLateral(Raw.Substring(1, Raw.Length - 2));
                return new Completion(str);
            }
            if (Raw.StartsWith("'") && Raw.EndsWith("'") && Raw.Length == 3)
                return new Completion(Raw[1]);
            if (Raw.StartsWith("0x", StringComparison.CurrentCultureIgnoreCase) ||
                Raw.StartsWith("&H", StringComparison.CurrentCultureIgnoreCase))
            {
                string hex = Raw.Substring(2);
                uint uintValue;
                if (uint.TryParse(hex, System.Globalization.NumberStyles.HexNumber, CultureInfo.CurrentCulture, out uintValue))
                    return new Completion(uintValue);
            }
            if (Raw.Equals("true", StringComparison.CurrentCultureIgnoreCase))
                return new Completion(true);
            if (Raw.Equals("false", StringComparison.CurrentCultureIgnoreCase))
                return new Completion(false);
            if (Raw.Equals("null", StringComparison.CurrentCultureIgnoreCase))
                return new Completion(null);
            if (int.TryParse(Raw, out intValue))
                return new Completion(intValue);
            if (float.TryParse(Raw, out floatValue))
                return new Completion(floatValue);
            if (double.TryParse(Raw, out doubleValue))
                return new Completion(doubleValue);
            if (Boolean.TryParse(Raw, out b))
                return new Completion(b);
            //if (enviroment.HasValue(Raw))
            //    return new Completion(enviroment.GetValue(Raw));
            return Completion.Exception(string.Format(Properties.Language.InvalidFormat, Raw ), this);
        }

        public Completion Assign(ExecutionEnvironment environment, object value)
        {
            if (environment.HasValue(Raw))
                environment.SetValue(Raw, value);
            return Completion.Exception(string.Format(Properties.Language.VariableNotDefined, Raw), this);
        }

        public override Descriptor Descriptor
        {
            get { throw new NotImplementedException(); }
        }

        public override string Type
        {
            get { return "Literal"; }
        }

        public string Raw
        {
            get;
            set;
        }

    }
}
