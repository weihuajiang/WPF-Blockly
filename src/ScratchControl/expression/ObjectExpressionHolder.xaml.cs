using ScratchNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScratchControl
{
    /// <summary>
    /// Interaction logic for ObjectExpressionHolder.xaml
    /// </summary>
    public partial class ObjectExpressionHolder : Grid, ExpressionHolder
    {
        public ObjectExpressionHolder()
        {
            InitializeComponent();
        }
        ExpressionDescriptor descriptor;
        public ExpressionDescriptor Descriptor
        {
            get
            {
                return descriptor;
            }
            set
            {
                descriptor = value;
                if (value != null)
                {
                    object exp = value.Value;
                    if (exp is ScratchNet.Expression)
                        SetExpression(exp as ScratchNet.Expression);
                }
            }
        }
        public void SetExpression(ScratchNet.Expression exp)
        {
            if (exp == null)
            {
                if (this.Children.Count > 3)
                    this.Children.RemoveAt(3);
                //this.TextInput.Visibility = Visibility.Visible;
                LeftPart.Visibility = Visibility.Visible;
                CenterPart.Visibility = Visibility.Visible;
                RightPart.Visibility = Visibility.Visible;
            }
            else
            {
                LeftPart.Visibility = Visibility.Collapsed;
                CenterPart.Visibility = Visibility.Collapsed;
                RightPart.Visibility = Visibility.Collapsed;
                if (this.Children.Count > 3)
                {
                    this.Children.RemoveAt(3);
                }
                if (exp.Type == "number")
                {
                    NumberExpressionControl ctrl = new NumberExpressionControl();
                    Grid.SetColumnSpan(ctrl, 3);
                    ctrl.Expression = exp;
                    this.Children.Add(ctrl);
                }
                else if (exp.Type == "boolean")
                {
                    BooleanExpressionControl ctrl = new BooleanExpressionControl();
                    Grid.SetColumnSpan(ctrl, 3);
                    ctrl.Expression = exp;
                    this.Children.Add(ctrl);
                }
                else
                {
                    ObjectExpressionControl ctrl = new ObjectExpressionControl();
                    Grid.SetColumnSpan(ctrl, 3);
                    ctrl.Expression = exp;
                    this.Children.Add(ctrl);
                }
            }
        }
        Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
        private bool IsTextAllowed(string text)
        {
            return !regex.IsMatch(text);
        }
        private void TextInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Descriptor != null && Descriptor.NothingAllowed)
            {
                e.Handled = true;
            }
            else if (Descriptor != null && Descriptor.IsOnlyNumberAllowed)
            {
                e.Handled = !IsTextAllowed(e.Text);
            }
        }

        private void TextInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (descriptor != null)
                descriptor.Value = TextInput.Text;
        }
    }
}
