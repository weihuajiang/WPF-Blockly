using ScratchNet;
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
    /// Interaction logic for SelectionItemControl.xaml
    /// </summary>
    public partial class SelectionItemControl : UserControl
    {
        public SelectionItemControl()
        {
            InitializeComponent();
        }
        SelectionItemDescriptor desc;
        public SelectionItemDescriptor Descriptor
        {
            set
            {
                desc = value;
                SelectionComboBox.ItemsSource = value.Texts;
                SelectionComboBox.SelectedValue = value.Value;
            }
            get
            {
                return desc;
            }
        }

        private void SelectionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object v = SelectionComboBox.SelectedValue;
            desc.Value = v;
        }
    }
}
