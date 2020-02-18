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
    /// Interaction logic for ResourceList.xaml
    /// </summary>
    public partial class ResourceList : UserControl
    {
        public ResourceList()
        {
            InitializeComponent();
        }
        ResourcesList _resources;
        public ResourcesList Resources
        {
            get
            {
                return _resources;
            }
            set
            {
                _resources = value;
                SpriteImageListView.ItemsSource = null;
                if (_resources != null)
                {
                    SpriteImageListView.ItemsSource = _resources;
                    this.IsEnabled = true;
                }
                else
                {
                    this.IsEnabled = false;
                }
            }
        }
        public bool IsSprite { get; set; }
        private void OnImportImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".png";
            dlg.Filter = "PNG Image (*.png)|*.png|JPEG Image (*.jpg)|*.jpg|SVG Image (*.svg)|*.svg|All Files (*.*)|*.*";
            dlg.Multiselect = false;
            dlg.InitialDirectory = System.IO.Directory.GetCurrentDirectory();

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string fileName = dlg.FileName;
                FileInfo finf = new FileInfo(fileName);
                int index = SpriteImageListView.SelectedIndex;
                string name = formatName(finf.Name);
                Resources.Add(new Resource() { FileName = fileName, DisplayName = name });
                SpriteImageListView.ItemsSource = null;
                SpriteImageListView.ItemsSource = Resources;
                SpriteImageListView.SelectedIndex = index;
                //if(IsSprite)
                    CurrentEnviroment.CurrentSpriteImages.Add(name);
                //else
               //     CurrentEnviroment.BackgroudImages.Add(name);
            }
        }

        private string formatName(string fileName)
        {
            int index = fileName.LastIndexOf(".");
            string name;
            if (index < 0)
                name = fileName;
            else
                name = fileName.Substring(0, index);
            int current = 1;
            string currentName = name;
            bool nameOk = true;
            while (true)
            {
                nameOk = true;
                foreach (Resource r in Resources)
                {
                    if (currentName == r.DisplayName)
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
        private void OnImportSound(object sender, RoutedEventArgs e)
        {

        }
    }
}
