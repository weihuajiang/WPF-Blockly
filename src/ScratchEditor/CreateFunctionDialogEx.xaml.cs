using System;
using System.CodeDom.Compiler;
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
    /// Interaction logic for CreateFunctionDialogEx.xaml
    /// </summary>
    public partial class CreateFunctionDialogEx : Window
    {
        public CreateFunctionDialogEx()
        {
            InitializeComponent();
            FunctionItems = new ObservableCollection<FunctionItem>();
            FunctionStructure.Background = new SolidColorBrush( EditorColors.Get<FunctionDeclaration>());
            FunctionStructure.ItemsSource = FunctionItems;
            FunctionNameText.Focus();
            FunctionNameText.SelectAll();
        }
        public List<string> ExistFunctions { get; private set; } = new List<string>();
        public ObservableCollection<FunctionItem> FunctionItems
        {
            get;
            set;
        }
        public string FunctionName
        {
            get
            {
                return FunctionNameText.Text;
            }
            set
            {
                FunctionNameText.Text = value;
            }
        }
        int n = 1;
        int b = 1;
        int str = 1;
        private void OnAddNumber(object sender, RoutedEventArgs e)
        {
            FunctionItems.Add(new NumberFunctionItem() { Value = "number" + n });
            n++;
        }

        private void OnAddString(object sender, RoutedEventArgs e)
        {
            FunctionItems.Add(new StringFunctionItem() { Value = "string" + str });
            str++;
        }

        private void OnAddBoolean(object sender, RoutedEventArgs e)
        {
            FunctionItems.Add(new BooleanFunctionItem() { Value = "bool" + b });
            b++;
        }

        private void OnAddText(object sender, RoutedEventArgs e)
        {
            FunctionItems.Add(new TextFunctionItem() { Value = "text" });
        }
        private void OnConfirm(object sender, RoutedEventArgs e)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("C#");
            if (!provider.IsValidIdentifier(FunctionName))
            {
                MessageBox.Show(Properties.Language.InvalidFuncName, Properties.Language.InvalidFuncName, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            foreach(var f in ExistFunctions)
            {
                if (f.Equals(FunctionName))
                {
                    MessageBox.Show(Properties.Language.FuncNameExisted, Properties.Language.InvalidFuncName, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            Dictionary<string, int> variables = new Dictionary<string, int>();
            foreach (FunctionItem item in FunctionItems)
            {
                if (!provider.IsValidIdentifier(item.Value))
                {
                    MessageBox.Show(string.Format(Properties.Language.ParameterNameInvalid, item.Value ), Properties.Language.InvalidParameterName, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                 if (!"text".Equals(item.Type))
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
                    MessageBox.Show(Properties.Language.ParameterNameSame, Properties.Language.InvalidParameterName, MessageBoxButton.OK, MessageBoxImage.Warning);
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
            TextBox b = sender as TextBox;
            lastObject = b.Tag;
            Point p = b.PointToScreen(new Point(b.ActualWidth / 2, 0));
            p = DeleteCanvas.PointFromScreen(p);
            DeleteButton.Visibility = Visibility.Visible;
            Canvas.SetLeft(DeleteButton, p.X - DeleteButton.ActualWidth / 2);
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
}
