using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for StatementControl.xaml
    /// </summary>
    public partial class StatementControl : UserControl, ActualSizeAdjustment, ISelectable
    {
        public StatementControl()
        {
            InitializeComponent();
            //StatementListView.DataContext = this;
        }
        public static readonly DependencyProperty StatementProperty =
            DependencyProperty.Register("Statement", typeof(Statement), typeof(StatementControl));
        public static readonly DependencyProperty DesignModeProperty =
            DependencyProperty.Register("DesignMode", typeof(Boolean), typeof(StatementControl));
        public static readonly DependencyProperty HoverOnTopProperty =
            DependencyProperty.Register("HoverOnTop", typeof(Boolean), typeof(StatementControl));
        public static readonly DependencyProperty HoverOnBottomProperty =
            DependencyProperty.Register("HoverOnBottom", typeof(Boolean), typeof(StatementControl));
        //new FrameworkPropertyMetadata(null, new PropertyChangedCallback(PatternPropertyChangedCallback)));
        public bool DesignMode
        {
            get
            {
                return (bool)this.GetValue(DesignModeProperty);
            }
            set
            {
                this.SetValue(DesignModeProperty, value);
            }
        }
        bool _isSelected = false;
        public bool IsSelected
        {
            set
            {
                _isSelected = value;
                if (value)
                    StatementListView.Effect = new DropShadowEffect() { ShadowDepth = 3 };
                else
                    StatementListView.Effect = null;
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
                return Statement;
            }
        }
        public Statement Statement
        {
            get
            {
                return (Statement)this.GetValue(StatementProperty);
            }
            set
            {
                this.SetValue(StatementProperty, value);
            }
        }
        public bool HoverOnTop
        {
            get
            {
                return (bool)this.GetValue(HoverOnTopProperty);
            }
            set
            {
                this.SetValue(HoverOnTopProperty, value);
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
            }
        }
        public bool HoverOnBottom
        {
            get
            {
                return (bool)this.GetValue(HoverOnBottomProperty);
            }
            set
            {
                this.SetValue(HoverOnBottomProperty, value);
            }
        }
    }
}
