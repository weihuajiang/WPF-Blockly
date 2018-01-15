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
using System.Windows.Shapes;

namespace ScratchNet
{
    /// <summary>
    /// Interaction logic for SetVariableNameDialog.xaml
    /// </summary>
    public partial class SetVariableNameDialog : Window
    {
        public SetVariableNameDialog()
        {
            InitializeComponent();
        }

        private void OnConfirm(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }
        public string Variable
        {
            get { return VariableName.Text.Trim(); }
        }
        private void OnCancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
