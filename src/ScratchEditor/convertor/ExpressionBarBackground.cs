using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace ScratchNet
{
    class ExpressionBarBackground : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value==null)
                return new SolidColorBrush(Colors.Green);
            Type type = value.GetType();
            return new SolidColorBrush( EditorColors.Get(type));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
