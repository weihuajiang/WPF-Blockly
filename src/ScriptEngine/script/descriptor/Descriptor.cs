using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ScratchNet
{
    public class Descriptor : List<ItemDescriptor>
    {
    }
    public class ItemDescriptor : INotifyPropertyChanged
    {
        public PropertyInfo Property { get;internal set; }
        public object Source { get; set; }
        public string Name { get; internal set; }
        public virtual object Value
        {
            get
            {
                object v = Property.GetValue(Source, null);
                if (v is Literal)
                {
                    Literal l = v as Literal;
                    if (l == null)
                        return "";
                    else
                        return l.Raw;

                }
                else
                {
                    return v;
                }
            }
            set
            {
                if (value is string)
                {
                    Literal l = new Literal() { Raw = value as string };
                    Property.SetValue(Source, l, null);
                }
                else
                {
                    Property.SetValue(Source, value, null);
                }
                OnPropertyChanged("Value");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        internal void OnPropertyChanged(string name){
            if(PropertyChanged!=null)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
    public class StringInputDesciptor : ItemDescriptor
    {
        public StringInputDesciptor(object source, string name)
        {
            Name = name;
            Source = source;

            Type t = Source.GetType();
            Property = t.GetProperty(name);
        }
        public override object Value
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
    }
    public class MultiLineStringInputDesciptor : ItemDescriptor
    {
        public MultiLineStringInputDesciptor(object source, string name)
        {
            Name = name;
            Source = source;

            Type t = Source.GetType();
            Property = t.GetProperty(name);
        }
        public override object Value
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
    }
    public class ParameterDescriptor : ItemDescriptor
    {
        public string Type { get; internal set; }
        public ParamDirection Direction { get; internal set; }
        public int Index { get; set; }
        public bool Editable { get; set; }
        public ParameterDescriptor(object source,int index, string name, string type, ParamDirection direction)
        {
            Source = source;
            Name = name;
            Type = type;
            Direction = direction;
            Editable = false;
        }
    }
    public class VariableDeclarationDescription : ParameterDescriptor
    {
        public VariableDeclarationDescription(object source, string name, string type) :base(source, -1, name, type, ParamDirection.Ref)
        {
            Editable = true;
        }
    }
    //
    // Summary:
    //     Describes how a child element is vertically positioned or stretched within a
    //     parent's layout slot.
    public enum VerticalAlignmentEnum
    {
        //
        // Summary:
        //     The child element is aligned to the top of the parent's layout slot.
        Top = 0,
        //
        // Summary:
        //     The child element is aligned to the center of the parent's layout slot.
        Center = 1,
        //
        // Summary:
        //     The child element is aligned to the bottom of the parent's layout slot.
        Bottom = 2,
        //
        // Summary:
        //     The child element stretches to fill the parent's layout slot.
        Stretch = 3
    }
    public class SelectionItemDescriptor : ItemDescriptor
    {
        public SelectionItemDescriptor(object source, string name, IList texts, IList values)
        {
            Name = name;
            Source = source;

            Type t = Source.GetType();
            Property = t.GetProperty(name);
            Texts = texts;
            Values = values;
            
        }
        public IList Texts { get; internal set; }
        public IList Values { get; internal set; }
        public override object Value
        {
            get
            {
                object v = Property.GetValue(Source);
                for (int i = 0; i < Values.Count; i++)
                {
                    if (Values[i].Equals(v) || Values[i]==v)
                    {
                        return Texts[i];
                    }
                }
                return null;
            }
            set
            {
                object v = value;
                for (int i = 0; i < Values.Count; i++)
                    if (Texts[i].Equals(v) || Texts[i]==v)
                    {
                        v = Values[i];
                        break;
                    }

                Property.SetValue(Source, v);
                OnPropertyChanged("Value");
            }
        }
    }
    public class TextItemDescriptor : ItemDescriptor
    {
        public TextItemDescriptor(object source, string text, bool isKeyword=false)
        {
            Text = text;
            Source = source;
            this.IsKeyword = isKeyword;
        }
        public VerticalAlignmentEnum VerticalAlignment { get; set; } = VerticalAlignmentEnum.Center;
        public bool IsKeyword { get; set; } = false;
        public string Text { get; internal set; }
    }
    public class ImageItemDescriptor : ItemDescriptor
    {
        public ImageItemDescriptor(object source, string path)
        {
            Path = path;
            Source = source;
        }
        public string Path { get; internal set; }
    }
    public class VariableDescriptor : ItemDescriptor
    {
        public VariableDescriptor(object source, string name)
        {
            Name = name;
            Source = source;

            Type t = Source.GetType();
            Property = t.GetProperty(name);
        }
        public override object Value
        {
            get
            {
                object v = Property.GetValue(Source, null);
                if (v is Identifier)
                {
                    Identifier l = v as Identifier;
                    if (l == null)
                        return "";
                    else
                        return l.Variable;

                }
                else
                {
                    return v;
                }
            }
            set
            {
                if (value == null)
                    return;
                if (value is string)
                {
                    Identifier l = new Identifier() { Variable = value as string };
                    Property.SetValue(Source, l, null);
                }
                else
                {
                    Property.SetValue(Source, value, null);
                }
                OnPropertyChanged("Value");
            }
        }
    }
    public class ExpressionDescriptor : ItemDescriptor
    {
        public ExpressionDescriptor(object source, string name, String type)
        {
            Name = name;
            Type = type;
            Source = source;

            Type t = Source.GetType();
            Property = t.GetProperty(name);
            IsOnlyNumberAllowed = false;
        }
        public bool AcceptVariableDeclaration { get; set; } = false;
        public bool IsOnlyNumberAllowed { get; set; }
        public bool NothingAllowed { get; set; } = false;
        public string Type { get; internal set; }
    }
    public class ArgumentDescriptor : ExpressionDescriptor
    {
        public ArgumentDescriptor(object source, int index, string name, String type):
            base(source, name, type)
        {
            this.Index = index;
            Name = name;
            Type = type;
            Source = source;

            Type t = Source.GetType();
            Property = t.GetProperty(name);
        }
        public int Index { get; set; }
        public override object Value
        {
            get
            {
                object c = Property.GetValue(Source, null);
                var v = (c as List<Expression>)[Index];
                if (v is Literal)
                {
                    Literal l = v as Literal;
                    if (l == null)
                        return "";
                    else
                        return l.Raw;

                }
                else
                {
                    return v;
                }
            }
            set
            {
                if (value is string)
                {
                    Literal l = new Literal() { Raw = value as string };
                    object c = Property.GetValue(Source, null);
                    (c as List<Expression>)[Index]=l;
                }
                else
                {
                    object c = Property.GetValue(Source, null);
                    (c as List<Expression>)[Index] = value as Expression;
                }
                OnPropertyChanged("Value");
            }
        }
    }
}