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
    /// Interaction logic for BlockIndicator.xaml
    /// </summary>
    public partial class BlockIndicator : UserControl
    {
        public BlockIndicator()
        {
            InitializeComponent();
            //HoverBorder.DataContext = this;
            //BorderLeft.DataContext = this;
            //BorderCenter.DataContext = this;
            //BorderRight.DataContext = this;
        }
        public static readonly DependencyProperty IsHoveredProperty =
            DependencyProperty.Register("IsHovered", typeof(Boolean), typeof(BlockIndicator));
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Brush), typeof(BlockIndicator));
        public Brush Color
        {
            get
            {
                return (Brush)this.GetValue(ColorProperty);
            }
            set
            {
                this.SetValue(ColorProperty, value);
            }
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
                    HoverBorder.Visibility = Visibility.Visible;
                }
                else
                {
                    HoverBorder.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
