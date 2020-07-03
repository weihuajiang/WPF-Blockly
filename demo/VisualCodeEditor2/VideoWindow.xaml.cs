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
    /// Interaction logic for VideoWindow.xaml
    /// </summary>
    public partial class VideoWindow : Window
    {
        public VideoWindow()
        {
            InitializeComponent();
        }

        private void MediaPlayer_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void VideoWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //MediaPlayer.Source = new Uri(@"pack://application:,,,/video2.mp4");
            MediaPlayer.Source = new Uri(@"C:\Users\weihuajiang\Desktop\VisualCodeEditor\video2.mp4");
            Console.WriteLine(MediaPlayer.Source);
            MediaPlayer.Play();
        }
    }
}
