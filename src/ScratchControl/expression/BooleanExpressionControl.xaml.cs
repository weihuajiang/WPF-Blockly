using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for BooleanExpressionControl.xaml
    /// </summary>
    public partial class BooleanExpressionControl : Grid, ExpressionControl
    {
        public BooleanExpressionControl()
        {
            InitializeComponent();
        }
        public Brush ExpressionBackground
        {
            set
            {
                Polygon1.Fill = value;
                Polygon2.Fill = value;
                CenterPart.Background = value;
            }
        }
        Brush borderBrush = new SolidColorBrush(Colors.LightGray);
        public Brush ExpressionBorderBrush
        {
            set
            {
                Polyline1.Stroke = value;
                Polyline2.Stroke = value;
                CenterPart.BorderBrush = value;
                borderBrush = value;
            }
            get
            {
                return borderBrush;
            }
        }
        ScratchNet.Expression expression;
        public ScratchNet.Expression Expression
        {
            get
            {
                return expression;
            }
            set
            {
                expression = value;
                if (value == null)
                {
                    Container.Children.Clear();
                    return;
                }
                ExpressionHelper.SetupExpressionControl(value, Container);
            }
        }

        public bool IsSelected
        {
            set
            {
                if (value)
                {
                    Polyline1.Stroke = GlobalResource.SelectionBorderBrush;
                    Polyline2.Stroke = GlobalResource.SelectionBorderBrush; ;
                    CenterPart.BorderBrush = GlobalResource.SelectionBorderBrush; ;
                }
                else
                {
                    Polyline1.Stroke = borderBrush;
                    Polyline2.Stroke = borderBrush;
                    CenterPart.BorderBrush = borderBrush;
                }
            }
        }

        public bool IsHovered
        {
            set
            {
                if (value)
                {
                    Polyline1.Stroke = GlobalResource.HoverBorderBrush;
                    Polyline2.Stroke = GlobalResource.HoverBorderBrush;
                    CenterPart.BorderBrush = GlobalResource.HoverBorderBrush;
                }
                else
                {
                    Polyline1.Stroke = borderBrush;
                    Polyline2.Stroke = borderBrush;
                    CenterPart.BorderBrush = borderBrush;
                }
            }
        }

        public Nullable<bool> CanDrop
        {
            set
            {
                if (value == true)
                {
                    Polyline1.Stroke = GlobalResource.CanDropBorderBrush;
                    Polyline2.Stroke = GlobalResource.CanDropBorderBrush; ;
                    CenterPart.BorderBrush = GlobalResource.CanDropBorderBrush; ;
                }
                else if (value == false)
                {
                    Polyline1.Stroke = GlobalResource.CanDropBorderBrush;
                    Polyline1.Stroke = GlobalResource.CanDropBorderBrush;
                    CenterPart.BorderBrush = GlobalResource.CanDropBorderBrush;
                }
                else
                {
                    Polyline1.Stroke = borderBrush;
                    Polyline1.Stroke = borderBrush;
                    CenterPart.BorderBrush = borderBrush;
                }
            }
        }
    }
}
