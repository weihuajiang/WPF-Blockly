using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
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
    /// Interaction logic for FunctionalControl.xaml
    /// </summary>
    public partial class FunctionControl : UserControl, ActualSizeAdjustment, ISelectable
    {
        public FunctionControl()
        {
            InitializeComponent();
            //FunctionListView.DataContext = this;
        }

        public static readonly DependencyProperty FunctionProperty =
            DependencyProperty.Register("Function", typeof(Function), typeof(FunctionControl));
        public Function Function
        {
            get { return (Function)GetValue(FunctionProperty); }
            set { SetValue(FunctionProperty, value); }
        }

        bool _isSelected = false;
        public bool IsSelected
        {
            set
            {
                _isSelected = value;
                if (value)
                    FunctionListView.Effect = new DropShadowEffect() { ShadowDepth = 3 };
                else
                    FunctionListView.Effect = null;
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
                return Function;
            }
        }
        public void GetActualSize(out double width, out double height)
        {
            width = 0;
            height = 0;
            FrameworkElement obj = Utility.FindChildByName(this, "ViewBorder");
            if (obj != null)
            {
                width = obj.ActualWidth;
                FrameworkElement obj2 = Utility.FindChildByName(this, "ViewBorder");
                if (obj2 != null)
                    height = obj.ActualHeight + obj2.ActualHeight;
            }
        }
    }
}
