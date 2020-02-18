using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScratchNet
{
    public class Literal : Expression
    {
        public string ReturnType
        {
            get { return ""; }
        }

        public Completion Execute(ExecutionEnvironment enviroment)
        {
            if (Raw == null)
                return new Completion(null);
            int intValue = 0;
            float floatValue = 0;
            double doubleValue = 0;
            Boolean b = false;
            if (int.TryParse(Raw, out intValue))
                return new Completion(intValue);
            if (float.TryParse(Raw, out floatValue))
                return new Completion(floatValue);
            if (double.TryParse(Raw, out doubleValue))
                return new Completion(doubleValue);
            if (Boolean.TryParse(Raw, out b))
                return new Completion(b);
            if (Raw.StartsWith("\"") && Raw.EndsWith("\""))
                return new Completion(Raw.Substring(1, Raw.Length - 2));
            return Completion.Exception(Raw + " is not valid", this); 
        }

        public Descriptor Descriptor
        {
            get { throw new NotImplementedException(); }
        }

        public string Type
        {
            get { return "Literal"; }
        }
        public object Value
        {
            get
            {
                try
                {
                    return double.Parse(Raw);
                }
                catch (Exception e) {
                }

                return Raw;
            }
        }
        public string Raw
        {
            get;
            set;
        }

    }
}
