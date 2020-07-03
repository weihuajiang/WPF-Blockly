using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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

namespace ScratchNet.About
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AbountWindow : Window
    {
        public AbountWindow()
        {
            InitializeComponent();
            VersionLabel.Text= Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private void OnConfirm(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void OnGoto(object sender, RoutedEventArgs e)
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = true;
            p.StartInfo.FileName = "https://www.huaruirobot.com";
            p.Start();
        }
        private void OnMailto(object sender, RoutedEventArgs e)
        {
            {
                Process p = new Process();
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.FileName = "mailto:info@huaruirobot.com";
                p.Start();
            }
        }
    }
}
