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
    /// Interaction logic for LibraryWindow.xaml
    /// </summary>
    public partial class LibraryWindow : Window
    {
        public LibraryWindow(List<LibraryItem> libs)
        {
            InitializeComponent();
            LibraryList.ItemsSource = libs;
        }

        private void OnOK(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void BuyAddon(object sender, RoutedEventArgs e)
        {

        }
    }
}
