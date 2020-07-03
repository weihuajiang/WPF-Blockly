using System;
using System.CodeDom.Compiler;
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
            VariableName.Focus();
        }

        private void OnConfirm(object sender, RoutedEventArgs e)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("C#");
            if (provider.IsValidIdentifier(Variable))
            {
                DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show(string.Format(Properties.Language.VariableNameInvalid, Variable), Properties.Language.InvalidVariableName, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
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
