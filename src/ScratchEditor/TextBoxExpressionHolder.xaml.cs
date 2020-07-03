using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScratchNet
{
    /// <summary>
    /// Interaction logic for TextBoxExpressionHolder.xaml
    /// </summary>
    public partial class TextBoxExpressionHolder : UserControl, ISelectable
    {
        public TextBoxExpressionHolder()
        {
            InitializeComponent();
            //GridHolder.DataContext = this;
            //ChildExpressionControl.DataContext = this;
            //TextBoxHolder.DataContext = this;
            //Polygon1.DataContext = this;
            //Polygon2.DataContext = this;
            //Border1.DataContext = this;
            //Border2.DataContext = this;
        }
        public bool CanPlaceVariableDeclaration
        {
            get
            {
                if (ExpressionDescriptor == null)
                    return false;
                return ExpressionDescriptor.AcceptVariableDeclaration;
            }
        }
        public static readonly DependencyProperty ExpressionDescriptorProperty =
            DependencyProperty.Register("ExpressionDescriptor", typeof(ExpressionDescriptor), typeof(TextBoxExpressionHolder));
        public ExpressionDescriptor ExpressionDescriptor
        {
            get
            {
                return (ExpressionDescriptor)this.GetValue(ExpressionDescriptorProperty);
            }
            set
            {
                this.SetValue(ExpressionDescriptorProperty, value);
            }
        }
        public static readonly DependencyProperty BkColorProperty =
            DependencyProperty.Register("BkColor", typeof(Brush), typeof(TextBoxExpressionHolder));
        public Brush BkColor
        {
            get { return (Brush)GetValue(BkColorProperty); }
            set { SetValue(BkColorProperty, value); }
        }

        bool _isSelected = false;
        public bool IsSelected
        {
            set
            {
                _isSelected = value;
                if (value)
                    ChildExpressionControl.Effect = new DropShadowEffect() { ShadowDepth = 3 };
                else
                    ChildExpressionControl.Effect = null;
            }
            get
            {
                return _isSelected;
            }
        }
        public Node SelectedValue
        {
            get
            {
                if (ExpressionDescriptor != null)
                {
                    return ExpressionDescriptor.Value as Node;
                }
                return null;
            }
        }
        public static readonly DependencyProperty IsHoveredProperty =
            DependencyProperty.Register("IsHovered", typeof(Boolean), typeof(TextBoxExpressionHolder));

        public bool IsDropTypeOK
        {
            get;
            set;
        }
        public bool IsHovered
        {
            get
            {
                return (bool)this.GetValue(IsHoveredProperty);
            }
            set
            {
                this.SetValue(IsHoveredProperty, value);
                if (value)
                {
                    if (IsDropTypeOK)
                        this.BorderBrush = new SolidColorBrush(Colors.Yellow);
                    else
                        this.BorderBrush = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    this.BorderBrush = null;
                }
            }
        }

        private void TextBoxHolder_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("application/expression"))
            {
                //IsHovered = true;
                e.Handled = true;
            }
        }
        Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
        private bool IsTextAllowed(string text)
        {
            return !regex.IsMatch(text);
        }
        private void TextBoxHolder_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (ExpressionDescriptor.NothingAllowed)
            {
                e.Handled = true;
            }
            if (ExpressionDescriptor.IsOnlyNumberAllowed)
            {
                e.Handled = !IsTextAllowed(e.Text);
            }
        }
    }
}
