using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ScratchNet
{
    class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string path = value + "";
            if (!path.StartsWith(System.IO.Path.DirectorySeparatorChar + ""))
                path = System.IO.Path.DirectorySeparatorChar + path;
            return new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + System.IO.Path.DirectorySeparatorChar + path));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
