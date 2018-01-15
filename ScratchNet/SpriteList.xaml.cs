using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace ScratchNet
{
    /// <summary>
    /// Interaction logic for SpriteList.xaml
    /// </summary>
    public partial class SpriteList : UserControl
    {
        public SpriteList()
        {
            InitializeComponent();
        }
        private void OnImportSprite(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".png";
            dlg.Filter = "PNG Image (*.png)|*.png|JPEG Image (*.jpg)|*.jpg|SVG Image (*.svg)|*.svg|All Files (*.*)|*.*";
            dlg.Multiselect = false;
            dlg.InitialDirectory = System.IO.Directory.GetCurrentDirectory();

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                Sprite sp = new Sprite();
                string fileName = dlg.FileName;
                FileInfo finf = new FileInfo(fileName);
                string name = formatName(finf.Name);
                sp.Name = name;
                sp.Images.Add(new Resource() { FileName = fileName, DisplayName = name });
                sp.X = 200;
                sp.Y = 150;
                CurrentEnviroment.Game.AddSprite(sp);

                SpriteListView.ItemsSource = CurrentEnviroment.Game.Sprites;
                SpriteListView.SelectedIndex = CurrentEnviroment.Game.Sprites.Count - 1;
                //BackgroundContainer.Content = CurrentEnviroment.Game.Background;
            }
        }
        private string formatName(string fileName)
        {
            int index = fileName.LastIndexOf(".");
            string name;
            if(index<0)
                name= fileName;
            else
                name=fileName.Substring(0, index);
            int current = 1;
            string currentName=name;
            bool nameOk = true;
            while (true)
            {
                nameOk = true;
                foreach (Sprite sp in CurrentEnviroment.Game.Sprites)
                {
                    if (currentName == sp.Name)
                    {
                        nameOk = false;
                    }
                }
                if (nameOk)
                    return currentName;
                currentName = name + current;
                current++;
            }
        }
    }
}
