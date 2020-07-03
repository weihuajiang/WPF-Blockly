using ScratchNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScratchControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("kernel32.dll")]
        public static extern Boolean AllocConsole();
        [DllImport("kernel32.dll")]
        public static extern Boolean FreeConsole();
        public MainWindow()
        {
            InitializeComponent();
            AllocConsole();
            NbExp.Expression = new ConditionalExpression() { Test=new Identifier(){Variable="abc"}, Consequent = new BinaryExpression(), Alternate = new Identifier() { Variable = "abc", VarType="boolean" } };
            BnExp.Expression = new ConditionalExpression();
            var control = ExpressionHelper.Build(new ConditionalExpression() { Test = new Identifier() { Variable = "abc" }, Consequent = new BinaryExpression(), Alternate = new Identifier() { Variable = "abc", VarType = "boolean" } });

            Editor.Children.Add(control as UIElement);
            Canvas.SetTop(control as UIElement, 200);
            Canvas.SetLeft(control as UIElement, 50); 
            var control2 = ExpressionHelper.Build(new BinaryExpression() );

            Editor.Children.Add(control2 as UIElement);
            Canvas.SetTop(control2 as UIElement, 250);
            Canvas.SetLeft(control2 as UIElement, 50);


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
