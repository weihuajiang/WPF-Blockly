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
    /// Interaction logic for BooleanExpressionHolder.xaml
    /// </summary>
    public partial class BooleanExpressionHolder : Grid, ExpressionHolder
    {
        public BooleanExpressionHolder()
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
                if (CenterPart.Children.Count > 3)
                    CenterPart.Children.RemoveAt(3);
                LeftPart.Visibility = Visibility.Visible;
                CenterPart.Visibility = Visibility.Visible;
                RightPart.Visibility = Visibility.Visible;
            }
            else
            {
                if (CenterPart.Children.Count > 3)
                {
                    CenterPart.Children.RemoveAt(3);
                }
                LeftPart.Visibility = Visibility.Collapsed;
                CenterPart.Visibility = Visibility.Collapsed;
                RightPart.Visibility = Visibility.Collapsed;
                
                if (descriptor.Type == "boolean")
                {
                    BooleanExpressionControl ctrl = new BooleanExpressionControl();
                    ctrl.Expression = exp;
                    this.Children.Add(ctrl);
                }
                //if (descriptor.Type == "number")
                {
                    NumberExpressionControl ctrl = new NumberExpressionControl();
                    ctrl.Expression = exp;
                    this.Children.Add(ctrl);
                }
                /*
                else
                {
                    ObjectExpressionControl ctrl = new ObjectExpressionControl();
                    ctrl.Expression = exp;
                    this.Children.Add(ctrl);
                }*/
            }
        }
    }
}
