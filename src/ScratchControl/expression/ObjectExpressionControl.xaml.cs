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
    /// Interaction logic for ObjectExpressionControl.xaml
    /// </summary>
    public partial class ObjectExpressionControl : Grid, ExpressionControl
    {
        public ObjectExpressionControl()
        {
            InitializeComponent();
        }
        public Brush ExpressionBackground
        {
            set
            {
                LeftPart.Background = value;
                RightPart.Background = value;
                CenterPart.Background = value;
            }
        }
        Brush borderBrush = new SolidColorBrush(Colors.LightGray);
        public Brush ExpressionBorderBrush
        {
            set
            {
                LeftPart.BorderBrush = value;
                RightPart.BorderBrush = value;
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
                    LeftPart.BorderBrush = GlobalResource.SelectionBorderBrush;
                    RightPart.BorderBrush = GlobalResource.SelectionBorderBrush; ;
                    CenterPart.BorderBrush = GlobalResource.SelectionBorderBrush; ;
                }
                else
                {
                    LeftPart.BorderBrush = borderBrush;
                    RightPart.BorderBrush = borderBrush;
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
                    LeftPart.BorderBrush = GlobalResource.HoverBorderBrush;
                    RightPart.BorderBrush = GlobalResource.HoverBorderBrush;
                    CenterPart.BorderBrush = GlobalResource.HoverBorderBrush;
                }
                else
                {
                    LeftPart.BorderBrush = borderBrush;
                    RightPart.BorderBrush = borderBrush;
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
                    LeftPart.BorderBrush = GlobalResource.CanDropBorderBrush;
                    RightPart.BorderBrush = GlobalResource.CanDropBorderBrush; ;
                    CenterPart.BorderBrush = GlobalResource.CanDropBorderBrush; ;
                }
                else if (value == false)
                {
                    LeftPart.BorderBrush = GlobalResource.CanDropBorderBrush;
                    RightPart.BorderBrush = GlobalResource.CanDropBorderBrush;
                    CenterPart.BorderBrush = GlobalResource.CanDropBorderBrush;
                }
                else
                {
                    LeftPart.BorderBrush = borderBrush;
                    RightPart.BorderBrush = borderBrush;
                    CenterPart.BorderBrush = borderBrush;
                }
            }
        }
    }
}
