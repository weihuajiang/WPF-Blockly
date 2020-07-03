using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ScratchNet
{
    public class BlockDescriptor : ObservableCollection<BlockItemDescriptor>
    {
    }
    public class BlockItemDescriptor : INotifyPropertyChanged
    {
        public PropertyInfo Property { get; internal set; }
        public object Source { get; set; }
        public string Name { get; internal set; }
        public virtual object Value
        {
            get
            {
                object v = Property.GetValue(Source, null);
                return v;
            }
            set
            {
                Property.SetValue(Source, value, null);
                OnPropertyChanged("Value");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        internal void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
    public class ExpressionStatementDescription : BlockItemDescriptor
    {
        public Descriptor Descriptor { get; set; }
        public ExpressionStatementDescription(object source, string name, Descriptor d)
        {
            Name = name;
            Source = source;
            Descriptor = d;
        }
        public override object Value
        {
            get
            {
                return Descriptor;
            }
            set
            {
                this.Descriptor = Descriptor;
            }
        }

    }
    public class TextBlockStatementDescritor : BlockItemDescriptor
    {
        public string Text { get; set; }
        public TextBlockStatementDescritor(object source, string name, string text)
        {
            Name = name;
            Source = source;
            Text = text;
            Type t = Source.GetType();
            Property = t.GetProperty(name);
        }
    }
    public class BlockStatementDescriptor : BlockItemDescriptor
    {
        public BlockStatementDescriptor(object source, string name, bool keyword=false)
        {
            Name = name;
            Source = source;
            IsKeyword = keyword;

            Type t = Source.GetType();
            Property = t.GetProperty(name);
        }
        public bool IsKeyword { get; set; } = false;
    }
}
