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
    /// Interaction logic for StatementBlockControl.xaml
    /// </summary>
    public partial class BlockStatementControl : UserControl
    {
        public BlockStatementControl()
        {
            InitializeComponent();
            //BlockStatementList.DataContext = this;
        }        
        public static readonly DependencyProperty BlockStatementProperty =
            DependencyProperty.Register("BlockStatement", typeof(BlockStatement), typeof(BlockStatementControl));         
        public static readonly DependencyProperty DesignModeProperty =
            DependencyProperty.Register("DesignMode", typeof(Boolean), typeof(BlockStatementControl));
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
        public BlockStatement BlockStatement
        {
            get
            {
                return (BlockStatement)this.GetValue(BlockStatementProperty);
            }
            set
            {
                this.SetValue(BlockStatementProperty, value);
            }
        }       
    }
}
