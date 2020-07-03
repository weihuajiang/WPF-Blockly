using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for EditorItemControl.xaml
    /// </summary>
    public partial class EditorItemControl : UserControl
    {
        GraphicScriptEditor _editor;
        public string Desciption { get; set; }
        public EditorItemControl(GraphicScriptEditor editor, string description=null)
        {
            InitializeComponent();
            _editor = editor;
            this.DataContext = this;
            Desciption = description;
        }
        public bool IsColorEditable
        {get;set;
        }
        Object _editorObject;
        public Object EditorObject
        {
            set
            {
                _editorObject = value;
                if(value is Expression)
                {
                    ExpressionControl control = new ExpressionControl();
                    control.Expression = (value as Expression);
                    Grid.SetColumn(control, 1);
                    if(!string.IsNullOrEmpty(Desciption))
                        control.ToolTip = Desciption;
                    ControlGrid.Children.Add(control);
                }
                else if( value is Statement)
                {
                    StatementControl control = new StatementControl();
                    control.Statement = (value as Statement);
                    Grid.SetColumn(control, 1);
                    if (!string.IsNullOrEmpty(Desciption))
                        control.ToolTip = Desciption;
                    ControlGrid.Children.Add(control);
                }
                else if(value is Function)
                {
                    FunctionControl control = new FunctionControl();
                    control.Function = value as Function;
                    Grid.SetColumn(control, 1);
                    if (!string.IsNullOrEmpty(Desciption))
                        control.ToolTip = Desciption;
                    ControlGrid.Children.Add(control);
                }
            }
            get
            {
                return _editorObject;
            }
        }

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            if(IsColorEditable)
                EditButton.Visibility = Visibility.Visible;
            e.Handled = false;
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            EditButton.Visibility = Visibility.Hidden;
        }
        bool selfChanging = false;
        private void OnEditColor(object sender, RoutedEventArgs e)
        {
        }
        public Action<Object, Color> ColorChanged;

        private void OnSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
        }
    }
}
