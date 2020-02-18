using Microsoft.Win32;
using ScratchEditor;
using ScratchNet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
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
            ConifgFileName =Environment.CurrentDirectory+"\\ "+"config.ini";
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
                    new LoopStatement(),
                    new WhileStatement(),
                    new BreakStatement(),
                    new ContinueStatement(),
                    new ReturnStatement(),
                    new TryStatement()
                }
            });
            toolbar.Add(new ScriptStepGroup()
            {
                Name = "Others",
                Types = new List<object>(){
                    new LogStatement(),
                    new WaitStatement(),
                    new ExpressionStatement()
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
            new Thread(() =>
            {
                Thread.Sleep(300);
                
                if (!string.IsNullOrEmpty(File) && System.IO.File.Exists(File))
                {
                    Dispatcher.InvokeAsync(() =>
                    {
                        Editor.Script = null;
                        Editor.IsEnabled = false;
                        Script script = Serialization.Load(File);
                        Editor.Script = script;
                        Editor.IsEnabled = true;
                        ButtonStart.IsEnabled = true;
                        ButtonOpen.IsEnabled = true;
                        ButtonSave.IsEnabled = true;
                    });
                }
            }).Start();

        }
        void WriteLineFunc(object obj, params object[] param)
        {
            Dispatcher.InvokeAsync(() =>
            {
                string text = string.Format(obj + "", param);
                consoles.Insert(0, new ConsoleItem() { Time = DateTime.Now, Text = text });
            });
        }


        //ExecutionEngine engine;
        private void OnStartRun(object sender, RoutedEventArgs e)
        {
            consoles.Clear();

            ExecutionEnvironment engine = new ExecutionEnvironment();
            ButtonStart.IsEnabled = false;
            ButtonStop.IsEnabled = true;
            Editor.IsEnabled = false;
            engine.ExecutionCompleted += Engine_ExecutionCompleted;
            engine.ExecutionAborted += Engine_ExecutionAborted;
            engine.ExecuteAsync(Editor.Script);
            Console.WriteLine("Start run");
            return;
        }

        private void Engine_ExecutionCompleted(object engine, object arg)
        {
            Dispatcher.InvokeAsync(() =>
            {
                Editor.IsEnabled = true;
                ButtonStart.IsEnabled = true;
                ButtonStop.IsEnabled = false;
            });
        }
        IErrorHighlight lastErrorEditor;
        private void Engine_ExecutionAborted(object engine, Completion arg)
        {
            if (arg != null)
            {
                Console.WriteLine(arg.ReturnValue);
                Console.WriteLine(arg.Location);
                Dispatcher.InvokeAsync(() =>
                {
                    var editor = Editor.FindEditorFor(arg.Location);
                    if (editor != null)
                    {
                        Console.WriteLine("last " + editor);
                        lastErrorEditor = (editor as IErrorHighlight);
                        lastErrorEditor?.HighlightError();
                    }
                    //var p = Editor.FindParent(arg.Location);
                   // var pe = Editor.FindEditorFor(p);
                });
            }
            Dispatcher.InvokeAsync(() =>
            {
                Editor.IsEnabled = true;
                ButtonStart.IsEnabled = true;
                ButtonStop.IsEnabled = false;
            });
        }

        private void OnStopRun(object sender, RoutedEventArgs e)
        {
            /*
            if (engine != null)
            {
                engine.ExecutionCompleted -= Engine_ExecutionCompleted;
                engine.Stop();
                engine = null;
            }
            */
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            OnStopRun(this, null);
            base.OnClosing(e);
        }
        string _currentFile;
        public string File
        {
            get
            {
                if(string.IsNullOrEmpty(_currentFile))
                    _currentFile = ReadValue("Editor", "LastOpen");
                return _currentFile;
            }
            set
            {
                _currentFile = value;
                WriteValue("Editor", "LastOpen", value);
            }
        }
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
            File = null;
            Editor.Script = new Script();
            Editor.IsEnabled = true;
            ButtonOpen.IsEnabled = true;
            ButtonSave.IsEnabled = true;
            ButtonStart.IsEnabled = true;
        }
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
            string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
                 string key, string def, StringBuilder retVal,
            int size, string filePath);
        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileSection(string lpAppName, byte[] lpszReturnBuffer, int nSize, string lpFileName);

        public void WriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, ConifgFileName);
        }

        public string ReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp,
                                            255, ConifgFileName);
            if (i <= 0)
                return null;
            String Value = temp.ToString();
            return Value;

        }
        /// <summary>
        /// Ini configuration file name
        /// </summary>
        string ConifgFileName = "config.ini";

        private void OnClearError(object sender, RoutedEventArgs e)
        {
            lastErrorEditor?.ClearHighlightError();
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="file">Ini configuration file name</param>
    }
}
