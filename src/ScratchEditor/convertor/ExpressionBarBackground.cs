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
        static Dictionary<string, Brush> StepColors;
        static void LoadColor()
        {
            StepColors = new Dictionary<string, Brush>();
            //StreamReader reader=new StreamReader(System.IO.Directory.GetCurrentDirectory()+@"\config\colors.config");
            //string line;
            //while((line=reader.ReadLine())!=null){
            //    string[] spt=line.Split(new string[]{":"}, StringSplitOptions.RemoveEmptyEntries);
            //    if(spt.Length<2)
            //        continue;
            //    try
            //    {
            //        StepColors.Add(spt[0].Trim(), new SolidColorBrush((Color)ColorConverter.ConvertFromString(spt[1].Trim())));
            //    }
            //    catch (Exception e) { }
            //}
            //reader.Close();

            string colors = ScratchEditor.Properties.Resources.colors;
            foreach (var line in colors.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
            {
                if (line.Length == 0) continue;
                string[] spt = line.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                if (spt.Length < 2)
                    continue;
                try
                {
                    StepColors.Add(spt[0].Trim(), new SolidColorBrush((Color)ColorConverter.ConvertFromString(spt[1].Trim())));
                }
                catch (Exception e) { }
            }
        }

        private List<string> GetLines(byte[] content)
        {
            string s = Encoding.Default.GetString(content, 0, content.Length - 1);
            return new List<string>(s.Split(new[] { Environment.NewLine }, StringSplitOptions.None));
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (StepColors == null)
                LoadColor();
            if(value==null)
                return new SolidColorBrush(Colors.Green);
            string type = value.GetType().ToString();
            if (StepColors.ContainsKey(type))
                return StepColors[type];
            return new SolidColorBrush(Colors.Green);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
