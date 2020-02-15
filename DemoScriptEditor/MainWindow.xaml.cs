using Microsoft.Win32;
using ScratchNet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace DemoScriptEditor
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
        ObservableCollection<ConsoleItem> consoles = new ObservableCollection<ConsoleItem>();
        public MainWindow()
        {
            AllocConsole();
            InitializeComponent();
            Editor.IsEnabled = false;
            List<ScriptStepGroup> toolbar = new List<ScriptStepGroup>();
            toolbar.Add(new ScriptStepGroup()
            {
                Name = "Logic",
                Types = new List<object>(){
                    new CompareExpression(),
                    new LogicExpression(),
                    new NotExpression()
                }
            });
            toolbar.Add(new ScriptStepGroup()
            {
                Name = "Number",
                Types = new List<object>(){
                    new BinaryExpression(),
                    new ConditionalExpression(),
                    new RandomExpression()
                }
            });
            toolbar.Add(new ScriptStepGroup()
            {
                Name = "Flow",
                Types = new List<object>(){
                    new IfStatement(),
                    new IfStatement(){Alternate=new BlockStatement()},
                    new ForStatement(),
                    new WhileStatement(),
                    new BreakStatement(),
                    new ContinueStatement(),
                    new ReturnStatement(),
                    new TryStatement()
                }
            });
            toolbar.Add(new ScriptStepGroup()
            {
                Name = "Console",
                Types = new List<object>(){
                    new LogStatement()
                }
            });
            /*
            toolbar.Add(new ScriptStepGroup()
            {
                Name = "Event",
                Types = new List<object>(){
                    //new StartEventHandler(),
                }
            });*/
            toolbar.Add(new ScriptStepGroup()
            {
                Name = "Variable",
                Types = new List<object>(){
                    "CreateVariable",
                    new AssignmentStatement(),
                }
            });
            toolbar.Add(new ScriptStepGroup()
            {
                Name = "Function",
                Types = new List<object>(){
                    "CreateFunction"
                }
            });
            Editor.SetToolBar(toolbar);
            LogStatement.WriteLine = WriteLineFunc;
            ConsoleList.ItemsSource = consoles;
        }
        void WriteLineFunc(object obj, params object[] param)
        {
            Dispatcher.InvokeAsync(() =>
            {
                string text = string.Format(obj + "", param);
                consoles.Insert(0, new ConsoleItem() { Time = DateTime.Now, Text = text });
            });
        }


        ExecutionEngine engine;
        private void OnStartRun(object sender, RoutedEventArgs e)
        {
            consoles.Clear();
            foreach(var func in Editor.Script.Functions)
            {
                if("main".Equals(func.Format, StringComparison.OrdinalIgnoreCase))
                {
                    if (engine != null)
                    {
                        engine.Stop();
                        engine = null;
                    }
                    engine = new ExecutionEngine();
                    engine.AddInstance(new Instance(Editor.Script));
                    engine.Start();
                    engine.RunFunction(func);
                    Console.WriteLine("Start run");
                    return;
                }
            }
            MessageBox.Show(this, "找不到main主方法", "无法运行", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void OnStopRun(object sender, RoutedEventArgs e)
        {
            if (engine != null)
            {
                engine.Stop();
                engine = null;
            }
        }
        public string File { get; set; }
        private void OnOpen(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".xml";
            dlg.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
            dlg.Multiselect = false;
            dlg.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
            ButtonOpen.IsEnabled = false;
            ButtonSave.IsEnabled = false;
            ButtonStart.IsEnabled = false;
            ButtonStop.IsEnabled = false;
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                Editor.Script = null;
                Editor.IsEnabled = false;
                File = dlg.FileName;
                Script script = Serialization.Load(File);
                Editor.Script = script;
                Editor.IsEnabled = true;
                ButtonStart.IsEnabled = true;
                ButtonStop.IsEnabled = true;
            }
            ButtonOpen.IsEnabled = true;
            ButtonSave.IsEnabled = true;
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            ButtonOpen.IsEnabled = false;
            ButtonSave.IsEnabled = false;
            if (string.IsNullOrEmpty(File))
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.DefaultExt = ".xml";
                dlg.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
                //dlg.Multiselect = false;
                dlg.InitialDirectory = System.IO.Directory.GetCurrentDirectory();

                Nullable<bool> result = dlg.ShowDialog();
                if (result != true)
                {
                    ButtonOpen.IsEnabled = true;
                    ButtonSave.IsEnabled = true;
                    return;
                }
                File = dlg.FileName;
            }
            Serialization.Save((Script)Editor.Script, File);
            ButtonOpen.IsEnabled = true;
            ButtonSave.IsEnabled = true;
        }

        private void OnNew(object sender, RoutedEventArgs e)
        {
            Editor.Script = new Script();
            Editor.IsEnabled = true;
            ButtonOpen.IsEnabled = true;
            ButtonSave.IsEnabled = true;
            ButtonStart.IsEnabled = true;
            ButtonStop.IsEnabled = true;
        }
    }
}
