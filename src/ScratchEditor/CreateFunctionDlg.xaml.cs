using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for CreateFunctionDlg.xaml
    /// </summary>
    public partial class CreateFunctionDlg : Window
    {
        public CreateFunctionDlg()
        {
            InitializeComponent();
            FunctionItems = new ObservableCollection<FunctionItem>();
            FunctionItems.Add(new TextFunctionItem() { Value = "text" });
            FunctionStructure.Background = new SolidColorBrush( EditorColors.Get<FunctionDeclaration>());
            FunctionStructure.ItemsSource = FunctionItems;
        }
        public ObservableCollection<FunctionItem> FunctionItems
        {
            get;
            set;
        }
        int n = 1;
        int b = 1;
        int str=1;
        private void OnAddNumber(object sender, RoutedEventArgs e)
        {
            FunctionItems.Add(new NumberFunctionItem() { Value = "number"+n});
            n++;
        }

        private void OnAddString(object sender, RoutedEventArgs e)
        {
            FunctionItems.Add(new StringFunctionItem() { Value = "string"+str });
            str++;
        }

        private void OnAddBoolean(object sender, RoutedEventArgs e)
        {
            FunctionItems.Add(new BooleanFunctionItem() { Value = "bool"+b });
            b++;
        }

        private void OnAddText(object sender, RoutedEventArgs e)
        {
            FunctionItems.Add(new TextFunctionItem() { Value = "text" });
        }
        private void OnConfirm(object sender, RoutedEventArgs e)
        {
            Dictionary<string, int> variables = new Dictionary<string, int>();
            foreach (FunctionItem item in FunctionItems)
            {
                if (string.IsNullOrEmpty(item.Value))
                {
                    MessageBox.Show("请将所有参数和文本都都填上内容！");
                    return;
                }
                if(!"text".Equals(item.Type))
                {
                    if (variables.ContainsKey(item.Value))
                        variables[item.Value] += 1;
                    else
                        variables[item.Value] = 1;
                }
            }
            foreach (string key in variables.Keys)
            {
                if (variables[key] > 1)
                {
                    MessageBox.Show("参数名字不能相同！");
                    return;
                }
            }
            DialogResult = true;
            this.Close();
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
        object lastObject;
        private void OnTextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox b=sender as TextBox;
            lastObject=b.Tag;
            Point p = b.PointToScreen(new Point(b.ActualWidth / 2, 0));
            p = DeleteCanvas.PointFromScreen(p);
            DeleteButton.Visibility = Visibility.Visible;
            Canvas.SetLeft(DeleteButton, p.X-DeleteButton.ActualWidth/2);
            Canvas.SetTop(DeleteButton, 0);
        }

        private void OnDelete(object sender, RoutedEventArgs e)
        {
            if (lastObject != null && lastObject is FunctionItem)
            {
                FunctionItem item = lastObject as FunctionItem;
                if (FunctionItems.Contains(item))
                    FunctionItems.Remove(item);
            }
            DeleteButton.Visibility = Visibility.Collapsed;
        }
    }
    public interface FunctionItem
    {
        string Value { get; set; }
        string Type { get;  }
    }
    public class TextFunctionItem : FunctionItem
    {
        public string Value { get; set; }
        public string Type { get { return "text"; } }
    }
    class NumberFunctionItem : FunctionItem
    {
        public string Value { get; set; }
        public string Type { get { return "number"; } }
    }
    class BooleanFunctionItem : FunctionItem
    {
        public string Value { get; set; }
        public string Type { get { return "boolean"; } }
    }
    class StringFunctionItem : FunctionItem
    {
        public string Value { get; set; }
        public string Type { get { return "string"; } }
    } 
}
