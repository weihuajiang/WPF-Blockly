using ScratchEditor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
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

namespace ScratchNet
{
    /// <summary>
    /// Interaction logic for ExpressionControl.xaml
    /// </summary>
    public partial class ExpressionControl : UserControl, IErrorHighlight
    {
        public ExpressionControl()
        {
            InitializeComponent();
            ExpressionListView.DataContext = this;
        }
        public static readonly DependencyProperty ExpressionProperty =
            DependencyProperty.Register("Expression", typeof(Expression), typeof(ExpressionControl));
        public static readonly DependencyProperty DesignModeProperty =
            DependencyProperty.Register("DesignMode", typeof(Boolean), typeof(ExpressionControl));
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
        public Expression Expression
        {
            get
            {
                return (Expression)this.GetValue(ExpressionProperty);
            }
            set
            {
                this.SetValue(ExpressionProperty, value);
            }
        }
 

        private void OnExprCtrlDragOver(object sender, DragEventArgs e)
        {
            if (DesignMode)
            {
                return;
            }
            if (e.Data.GetDataPresent("application/expression"))
            {
                e.Handled = true;
            }
        }
        public void HighlightError()
        {
            HighlightHelper.HighlightError(this);
        }

        public void ClearHighlightError()
        {
            HighlightHelper.ClearHighlightError();
        }
    }
    
}
