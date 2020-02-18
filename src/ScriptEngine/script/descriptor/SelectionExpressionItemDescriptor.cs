using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class SelectionExpressionItemDescriptor : ItemDescriptor
    {
        public SelectionExpressionItemDescriptor(object source, string name, IList values, ExpressionDescriptor desc)
        {
            Name = name;
            Source = source;
            Descriptor = desc;

            Type t = Source.GetType();
            Property = t.GetProperty(name);
            Values = values;
        }
        public ExpressionDescriptor Descriptor{get;internal set;}
        public IList Values { get; internal set; }
        public override object Value
        {
            get
            {
                return Descriptor.Value;
            }
            set
            {
                if (value == null)
                    return;
                Descriptor.Value = value;
            }
        }
    }
}
