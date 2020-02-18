using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScratchNet
{
    /// <summary>
    /// Interaction logic for ParameterIndicator.xaml
    /// </summary>
    public partial class ParameterIndicator : UserControl
    {
        public ParameterIndicator()
        {
            InitializeComponent();
            ParamNameTextBox.DataContext = this;
            Polygon1.DataContext = this;
            Polygon2.DataContext = this;
            Border1.DataContext = this;
            Border2.DataContext = this;
        }
        public static readonly DependencyProperty ParameterProperty =
            DependencyProperty.Register("Parameter", typeof(ParameterDescriptor), typeof(ParameterIndicator));
        public ParameterDescriptor Parameter
        {
            get { return (ParameterDescriptor)GetValue(ParameterProperty); }
            set { SetValue(ParameterProperty, value); }
        }
    }
}
