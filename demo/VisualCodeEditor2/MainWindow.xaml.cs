using Microsoft.Win32;
using ScratchNet;
using ScratchNet.About;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToastNotifications.Core;

namespace ScratchNet
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
        Stack<Node> stackTrace = new Stack<Node>();
        bool IsStackMonitored = true;
        Toast toast;
        List<LibraryItem> libs = new List<LibraryItem>();
        static internal ImageSource GetImageSourceFromResource(string psAssemblyName, string psResourceName)
        {
            Uri oUri = new Uri("pack://application:,,,/" + psAssemblyName + ";component/" + psResourceName, UriKind.RelativeOrAbsolute);
            return BitmapFrame.Create(oUri);
        }
        static ImageSource LoadSource(string uri)
        {
            string assembly = typeof(MainWindow).Assembly.GetName().Name;
            return GetImageSourceFromResource(assembly, uri);
        }
        public MainWindow()
        {
            InitializeComponent();
            //AllocConsole();
            libs.Add(new LibraryItem() { Library = new MathLibary(), IsStoreLibrary=false, Image = LoadSource("/images/Math.png") });
            libs.Add(new LibraryItem() { Library = new CanvasCollection(), IsStoreLibrary = false, Image = LoadSource("/images/PAINT.png") });
            libs.Add(new LibraryItem() { Library = new SpeechLibrary(), IsStoreLibrary = false, Image = LoadSource("/images/Microphone.png") });
            libs.Add(new LibraryItem() { Library = new CollectionLibary(), IsStoreLibrary = false, Image = LoadSource("/images/list.png") });
            libs.Add(new LibraryItem() { Library = new DataStructureLibrary(), IsStoreLibrary = false, Image = LoadSource("/images/collection.png") });
            libs.Add(new LibraryItem() { Library = new ThreadCollection(), IsStoreLibrary = false, Image = LoadSource("/images/th.png") });

            Editor.IsEnabled = false;
            Editor.IsLibraryEnabled = true;
            Editor.Register((Color)ColorConverter.ConvertFromString("#95f0c7"), typeof(StringExpression), typeof(IndexOfStringExpression),
                typeof(LastIndexOfStringExpression), typeof(SubStringExpression), typeof(StringLengthExpression),  typeof(ParseLongExpression),
                typeof(ParseIntExpression), typeof(ParseFloatExpression), typeof(ParseDoubleExpression));
            Editor.Register((Color)ColorConverter.ConvertFromString("#4C97FF"), typeof(PrintLnStatement), typeof(PrintStatement),
                typeof(ReadLineExpression), typeof(ReadExpression), typeof(ClearStatement));
            Editor.Register<IfStatement, TrueExpression, FalseExpression>((Color)ColorConverter.ConvertFromString("#078aab"));
            Editor.Register((Color)ColorConverter.ConvertFromString("#06cccc"), typeof(NewArrayExpression), typeof(ArrayValueExpression), 
                typeof(NewArray2Expression),typeof(Array2ValueExpression),
                typeof(ArrayLengthExpression));
            foreach(var l in libs)
            {
                Color c = (Color)ColorConverter.ConvertFromString(l.Library.DefaultColor);
                foreach(var g in l.Library)
                {
                    foreach(var s in g)
                    {
                        Editor.Register(c, s.Step.GetType());
                    }
                }
            }
            Editor.SetToolBar(CreateToolbar());
            Editor.ScacleRatio = ScaleRatio;
            Editor.TypeColorChanged += EditTypeColorChanged;
            Editor.ChangeImportLibrary += Editor_ChangeImportLibrary;
            if ("true".Equals(IniFile.ReadValue("Editor", "IsHighLight"), StringComparison.OrdinalIgnoreCase))
                ButtonHighligh.IsChecked = true;
            string speed = IniFile.ReadValue("Editor", "HighLightSpeed");
            if (!string.IsNullOrEmpty(speed))
            {
                try
                {
                    SliderSpeeder.Value = 100 - double.Parse(speed);
                }
                catch { }
            }
            toast = new Toast();
            Unloaded += MainWindow_Unloaded;
            Loaded += MainWindow_Loaded;
            LoadColors();
            string firstTime = IniFile.ReadValue("Editor", "FirstUsage");
            if (string.IsNullOrEmpty(firstTime))
            {
                IniFile.WriteValue("Editor", "FirstUsage", "false");
                new Thread(() =>
                {
                    Dispatcher.InvokeAsync(() =>
                    {
                       MessageBoxResult ret= MessageBox.Show(this, Properties.Resources.RequestVideo, Properties.Resources.Welcome, MessageBoxButton.YesNo, MessageBoxImage.Information);
                        if(ret== MessageBoxResult.Yes)
                        {
                            OnHelp(null, null);
                        }
                    });
                }).Start();
            }
            else if (!string.IsNullOrEmpty(File) && System.IO.File.Exists(File))
            {
                new Thread(() =>
                {
                    Thread.Sleep(500);
                    Dispatcher.Invoke(() =>
                    {
                        Editor.Script = null;
                        Editor.IsEnabled = false;
                        try
                        {
                            Script script = Serialization.Load(File) as Script;
                            SetupScriptToolbar(script);
                            Editor.Script = script;
                            this.Title = Properties.Resources.VisualCodeEditor+" - " + File;
                            ShowMessage(toast.ShowSuccess, string.Format(Properties.Resources.SuccessLodFile, File));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.WriteLine(ex.StackTrace);
                            ShowMessage(toast.ShowWarning, string.Format(Properties.Resources.ExceptionLoadFile, File));
                            return;
                        }
                        Editor.IsEnabled = true;
                        ButtonStart.IsEnabled = true;
                        ButtonOpen.IsEnabled = true;
                        ButtonSave.IsEnabled = true;
                        ButtonSaveAs.IsEnabled = true;
                        ButtonPrint.IsEnabled = true;
                            //d.PurchaseAddOn("9NG2QVSXT34H");
                    });
                }).Start();
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var l in libs)
            {
                if (l.IsStoreLibrary)
                {
                    var s = IniFile.ReadValue("Addon", l.Assembly);
                    if (!string.IsNullOrEmpty(s))
                        l.IsPurchased = true;
                }
            }
        }
        void SetupScriptToolbar(Script script)
        {
            if (script != null)
            {
                foreach (var l in libs)
                {
                    var used = false;
                    foreach (var s in script.Imports)
                    {
                        if (l.Library.Name.Equals(s))
                            used = true;
                    }
                    l.IsImported = used;
                }
            }
            else
            {
                foreach (var l in libs)
                {
                    l.IsImported = false;
                }
            }
            Editor.SetToolBar(CreateToolbar());
        }
        private void Editor_ChangeImportLibrary(object sender, EventArgs e)
        {
            foreach(var l in libs)
            {
                var used = false;
                Script script = Editor.Script as Script;
                foreach (var s in script.Imports){
                    if (l.Library.Name.Equals(s))
                        used = true;
                }
                l.IsImported = used;
            }
            LibraryWindow libWnd = new LibraryWindow(libs);
            libWnd.Owner = this;
            var ret=libWnd.ShowDialog();
            if(ret.HasValue && ret.Value)
            {
                Script script = Editor.Script as Script;
                script.Imports.Clear();
                List<ScriptStepGroup> toolbar = Editor.GetToolBar();
                foreach (var l in libs)
                {
                    bool imprted = false;
                    ScriptStepGroup lbar = null;
                    foreach (var g in toolbar)
                    {
                        if (g.Name.Equals(l.Library.Title))
                        {
                            imprted = true;
                            lbar = g;
                        }
                    }
                    if (l.IsImported)
                    {
                        script.Imports.Add(l.Library.Name);
                        if (!imprted)
                            toolbar.Add(GetCommandsFromLib(l.Library));
                    }
                    else
                    {
                        if (imprted)
                            toolbar.Remove(lbar);
                    }
                }
                Editor.SetToolBar(toolbar);
            }
        }

        ScriptStepGroup GetCommandsFromLib(Library l)
        {
            ScriptStepGroup g = new ScriptStepGroup();
            g.Types = new List<object>();
            g.Name = l.Title;
            foreach(var gp in l)
            {
                g.Types.Add(new Label() { Content = gp.Name, ToolTip = gp.Description });
                foreach(var s in gp)
                {
                    g.Types.Add(new ScriptStep(s.Step, s.IsColorEditable, s.Description));
                }
            }
            return g;
        }
        List<ScriptStepGroup> CreateToolbar()
        {
            List<ScriptStepGroup> toolbar = new List<ScriptStepGroup>();
            toolbar.Add(new ScriptStepGroup()
            {
                Name = Properties.Resources.CommentCollection,
                Types = new List<object>() {
                    new Label(){Content=Properties.Resources.CommentCategory, ToolTip=Properties.Resources.CommentDescription},
                    new ScriptStep(new CommentStatement(), true, Properties.Resources.CommentDescription1),
                    new ScriptStep(new CommentStatement(){AllowMultiLine=true }, true,Properties.Resources.CommentDescription2)
                }
            });

            toolbar.Add(new ScriptStepGroup()
            {
                Name = Properties.Resources.StatementCollection,
                Types = new List<object>(){
                    new Label(){Content=Properties.Resources.ExpressionStatementCategory},
                     new ScriptStep(new ExpressionStatement(), true, Properties.Resources.ExpressionStatementDescription),

                    new Label(){Content=Properties.Resources.IfCategory,  ToolTip=Properties.Resources.IfDescription},
                    new ScriptStep(new IfStatement(), true, Properties.Resources.IfStatementDescription),
                    new ScriptStep(new IfStatement(){Alternate=new BlockStatement()}, true, Properties.Resources.IfElseStatementDescription),

                    new Label(){Content=Properties.Resources.LoopCategory},
                    new ScriptStep(new ForStatement(),true,Properties.Resources.ForDescription),
                    new ScriptStep(new WhileStatement(),true, Properties.Resources.WhileDescription),
                    new ScriptStep(new DoStatement(),true,Properties.Resources.DoWhileDescription),
                    new ScriptStep(new BreakStatement(),true,Properties.Resources.BreakDescription),
                    new ScriptStep(new ContinueStatement(),true,Properties.Resources.ContinueDescription),

                    new Label(){Content=Properties.Resources.ReturnCategory},
                    new ScriptStep(new ReturnStatement(),true, Properties.Resources.ReturnDescription),
                    new ScriptStep(new ReturnStatement(){Expression=new Literal()}, true, Properties.Resources.ReturnValueDescription),

                    new Label(){Content=Properties.Resources.ExceptionCategory},
                    new ScriptStep(new TryStatement(),true,Properties.Resources.TryDescription),
                    //new ScriptStep(new TryStatement(){Finally=new BlockStatement()},true, "try-catch with finally statement")
                }
            });
            toolbar.Add(new ScriptStepGroup()
            {
                Name = Properties.Resources.OperatorCollection,
                Types = new List<object>(){
                    new Label(){Content=Properties.Resources.LogicCategory},
                    new ScriptStep(new BinaryExpression(){ ValueType="boolean", Operator= Operator.Equal },true, Properties.Resources.BinaryExpressionDescription),//compare operator
                    new ScriptStep(new BinaryExpression(){ValueType="boolean", Operator= Operator.And },true,Properties.Resources.BinaryExpressionDescription),//logical operator
                    new ScriptStep(new NotExpression(),true, Properties.Resources.NotExpressionDescription),

                    new Label(){Content=Properties.Resources.NumberOperatorCategory},
                    new ScriptStep(new BinaryExpression(), true, Properties.Resources.BinaryExpressionDescription),
                    new ScriptStep(new UpdateExpression(), true, Properties.Resources.UpdateExpressionDescription),
                    new ScriptStep(new UpdateExpression(){IsPrefix=true}, true, Properties.Resources.UpdateExpressionDescription),

                    new Label(){Content=Properties.Resources.ConditionExpressionCategory},
                    new ScriptStep(new ConditionalExpression(), true,Properties.Resources.ConditionExpressionDescription),

                    new Label(){Content=Properties.Resources.VariableAssignmentCategory  },
                    new ScriptStep(new ExpressionStatement(){Expression=new AssignmentExpression()}, false, Properties.Resources.VariableAssignmentDescription),
                    new ScriptStep(new AssignmentExpression(),true, Properties.Resources.VariableAssignmentDescription)
                }
            });
            toolbar.Add(new ScriptStepGroup()
            {
                Name =Properties.Resources.VariableTypeCollection,
                Types = new List<object>() {
                    new Label() { Content = Properties.Resources.VariableDefCategory },
                        new ExpressionStatement() { Expression = new VariableDeclarationExpression() { CanAssignValue = false } },
                        new ExpressionStatement() { Expression = new VariableDeclarationExpression() },
                        new ScriptStep(new VariableDeclarationExpression() { CanAssignValue = false }, true, Properties.Resources.VariableDecDescription),
                        new ScriptStep(new VariableDeclarationExpression(), true, Properties.Resources.VariableDecDescription2),

                        new Label() { Content = Properties.Resources.VariableAssignmentCategory },
                        new ExpressionStatement() { Expression = new AssignmentExpression() },
                        new ScriptStep(new AssignmentExpression(), true, Properties.Resources.VariableAssignmentDescription),

                        new Label(){Content=Properties.Resources.NullCategory},
                        new ScriptStep(new NullExpression(),true),

                        new Label(){Content=Properties.Resources.LogicValueCategory},
                        new ScriptStep(new TrueExpression(),true),
                        new ScriptStep(new FalseExpression(), true),

                        new Label() { Content = Properties.Resources.TypeConvertCategory },
                        new ScriptStep(new ParseIntExpression(), true,  Properties.Resources.ParseIntDescription),
                        new ScriptStep(new ParseLongExpression(), true,  Properties.Resources.ParseIntDescription),
                        new ScriptStep(new ParseFloatExpression(), true, Properties.Resources.ParseFloatDescription),
                        new ScriptStep(new ParseDoubleExpression(), true, Properties.Resources.ParseDoubleDescription),

                        new Label() { Content = Properties.Resources.ArrayCollection },
                        new ScriptStep(new NewArrayExpression(), true, Properties.Resources.NewArrayDescription),
                        new ScriptStep(new NewArray2Expression(), true, Properties.Resources.NewArray2Description),
                        new ScriptStep(new ArrayValueExpression(), true, Properties.Resources.ArrayValueDescription),
                        new ScriptStep(new Array2ValueExpression(), true, Properties.Resources.ArrayValueDescription),
                        new ScriptStep(new ArrayLengthExpression(), true, Properties.Resources.ArrayLengthDescription),

                        new Label() { Content = Properties.Resources.StringCategory },
                        new ScriptStep(new StringExpression(), true, Properties.Resources.NewStringDescription),
                        new ScriptStep(new StringLengthExpression(), true, Properties.Resources.StringLengthDescription),
                        new ScriptStep(new IndexOfStringExpression(), true, Properties.Resources.StringIndexDescription),
                        new ScriptStep(new LastIndexOfStringExpression(), true, Properties.Resources.StringLastIndexDescription),
                        new ScriptStep(new SubStringExpression(), true, Properties.Resources.StringSubDescription),
                    }
            });
            toolbar.Add(new ScriptStepGroup()
            {
                Name = Properties.Resources.FunctionCollection,
                Types = new List<object>(){
                    new Label(){Content= Properties.Resources.FunctionNewCategory  },
                    "CreateFunction",
                    new Label(){Content= Properties.Resources.FunctionCallCategory  }
                }
            });
            toolbar.Add(new ScriptStepGroup()
            {
                Name = Properties.Resources.IOCollection,
                Types = new List<object>(){
                    new Label(){Content=Properties.Resources.ConsoleCategory},
                    new ScriptStep(new PrintLnStatement(), true,  Properties.Resources.PrintLnDescription),
                    new ScriptStep(new PrintStatement(), true, Properties.Resources.PrintDescription),
                    new ScriptStep(new ReadLineExpression(), true,Properties.Resources.ReadLnDescription),
                    new ScriptStep(new ReadExpression(), true, Properties.Resources.ReadDescripiton),
                    new ScriptStep(new ClearStatement(), true, Properties.Resources.ClearDescription)
                    //new Label(){Content="file"}
                }
            });
            foreach(var l in libs)
            {
                if(l.IsImported)
                    toolbar.Add(GetCommandsFromLib(l.Library));
            }
            return toolbar;
        }
        protected override void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi)
        {
            base.OnDpiChanged(oldDpi, newDpi);
        }
        private void LoadColors()
        {
            if (!System.IO.File.Exists(IniFile.ConfigFileName))
                return;
            using (StreamReader reader = new StreamReader(new FileStream(IniFile.ConfigFileName, FileMode.Open, FileAccess.ReadWrite)))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!line.Contains(",") || !line.Contains("="))
                        continue;
                    string[] splts = line.Split('=');
                    try
                    {
                        Editor.Register((Color)ColorConverter.ConvertFromString(splts[1]), Type.GetType(splts[0]));
                    }
                    catch { }
                }
                reader.Close();
            }
        }
        private static string GetTypeName(Type t)
        {
            string[] str = t.AssemblyQualifiedName.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            return str[0] + ", " + str[1];
        }
        private void EditTypeColorChanged(object sender, TypeColorChangeEventArgs e)
        {
            Editor.Register(e.Color, e.Type.GetType());
            IniFile.WriteValue("Step Color", GetTypeName(e.Type.GetType()), e.Color.ToString());
            var cs = Editor.Script;
            var modified = Editor.IsModified;
            Editor.Script = null;
            Editor.Script = cs;
            Editor.IsModified = modified;
        }

        string _lastMessage;
        private int _count = 0;
        void ShowMessage(Action<string, MessageOptions> action, string name)
        {
            MessageOptions opts = new MessageOptions
            {
                CloseClickAction = CloseAction,
                Tag = $"[This is Tag Value ({++_count})]",
                FreezeOnMouseEnter = true,
                UnfreezeOnMouseLeave = true,
                ShowCloseButton = true
            };
            _lastMessage = $"{name}";
            action(_lastMessage, opts);
        }

        private void CloseAction(NotificationBase obj)
        {
            var opts = obj.DisplayPart.Notification.Options;
        }
        private void MainWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            toast.OnUnloaded();
        }
        #region execute
        public bool IsHighlightStep { get; set; } = true;
        ExecutionEnvironment engine;
        //ExecutionEngine engine;
        private void OnStartRun(object sender, RoutedEventArgs e)
        {
            Editor.ClearHighlight();
            stackTrace.Clear();
            //check main function
            bool hasMain = false;
            foreach(var f in Editor.Script.Functions)
            {
                if(f.Name.Equals("main", StringComparison.OrdinalIgnoreCase))
                {
                    hasMain = true;
                    break;
                }
            }
            if (!hasMain)
            {
                MessageBox.Show(Properties.Resources.MainNotFound, Properties.Resources.NoMain, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            ButtonClearError.IsEnabled = false;
            IsHighlightStep = ButtonHighligh.IsChecked.Value;
            stackTrace.Clear();
            engine = new ExecutionEnvironment();
            engine.EnterNode += Engine_EnterNode;
            engine.LeaveNode += Engine_LeaveNode;
            ButtonStart.IsEnabled = false;
            ButtonStop.IsEnabled = true;
            //Editor.IsEnabled = false;
            engine.ExecutionCompleted += Engine_ExecutionCompleted;
            engine.ExecutionAborted += Engine_ExecutionAborted;
            engine.ExecuteAsync(Editor.Script);
            Console.WriteLine("Start run");
            return;
        }

        private void Engine_LeaveNode(object engine, ExecutionLeaveEventArgs arg)
        {
            if (IsStackMonitored && stackTrace.Count > 0)
            {
                stackTrace.Pop();
            }
            if (IsHighlightStep)
            {
                if (arg.Location != null && arg.Location is ScratchNet.Expression)
                {
                    Control editor = null;
                    double cycle = 100;
                    Dispatcher.Invoke(() =>
                    {
                        try
                        {
                            cycle = 100-SliderSpeeder.Value;
                            editor = Editor.FindEditorFor(arg.Location);
                            if (editor != null)
                            {
                                Editor.Highlight(editor, arg.Value == null ? null : arg.Value.ReturnValue);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            Console.WriteLine(e.StackTrace);
                        }
                    });
                    if (editor != null)
                    {
                        Thread.Sleep((int)(5000 * cycle / 100));
                        Dispatcher.Invoke(() =>
                        {
                            Editor.ClearHighlight();
                        });
                    }
                }
            }
        }
        private void Engine_EnterNode(object obj, ExecutionEnterEventArgs arg)
        {
            if (IsStackMonitored)
            {
                stackTrace.Push(arg.Location);
                if (stackTrace.Count > 1000)
                {
                    MessageBox.Show(Properties.Resources.StackMessage, Properties.Resources.StackTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                    engine.Stop();
                }
            }
            if(IsHighlightStep && arg.Location!=null && arg.Location is Statement)
            {
                Control editor = null;
                double cycle = 100;
                Dispatcher.Invoke(() =>
                {
                    try
                    {
                        cycle = 100-SliderSpeeder.Value;
                        editor = Editor.FindEditorFor(arg.Location);
                        if (editor != null)
                        {
                            Editor.Highlight(editor);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine(e.StackTrace);
                    }
                });
                if (editor != null)
                {
                    Thread.Sleep((int)(3000 * cycle / 100));
                    Dispatcher.Invoke(() =>
                    {
                        Editor.ClearHighlight();
                    });
                }
            }
        }

        private void Engine_ExecutionCompleted(object e, object arg)
        {
            PrintLnStatement.WriteLine("");
            PrintLnStatement.WriteLine(Properties.Resources.ApplicationComplete);
            PrintLnStatement.Stop();
            Dispatcher.InvokeAsync(() =>
            {
                Editor.IsEnabled = true;
                ButtonStart.IsEnabled = true;
                ButtonStop.IsEnabled = false;
                ButtonSaveAs.IsEnabled = true;
                ButtonPrint.IsEnabled = true;
            });
        }
        private void Engine_ExecutionAborted(object sender, Completion arg)
        {
            PrintLnStatement.WriteLine("");
            PrintLnStatement.WriteLine(Properties.Resources.ApplicationAbort);
            PrintLnStatement.WriteLine(arg.ReturnValue);
            PrintLnStatement.Stop();
            if (arg != null)
            {
                Dispatcher.InvokeAsync(() =>
                {
                    var ret = MessageBox.Show(Properties.Resources.ExceptionDuringRun+"\n" + arg.ReturnValue + "\n"+Properties.Resources.LocateException,
                        Properties.Resources.Exception, MessageBoxButton.YesNo, MessageBoxImage.Error);
                    if (ret == MessageBoxResult.Yes)
                    {
                        var editor = Editor.FindEditorFor(arg.Location);
                        if (editor == null)
                        {
                            var parent = Editor.FindParent(arg.Location);
                            editor=Editor.FindEditorFor(parent);
                        }
                        if (editor != null)
                        {
                            Console.WriteLine(editor);
                            Editor.Highlight(editor);
                            ButtonClearError.IsEnabled = true;
                        }
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
                ButtonSaveAs.IsEnabled = true;
                ButtonPrint.IsEnabled = true;
            });
        }

        private void OnStopRun(object sender, RoutedEventArgs e)
        {
            
            if (engine != null)
            {
                engine.EnterNode -= Engine_EnterNode;
                engine.LeaveNode -= Engine_LeaveNode;
                //Editor.IsEnabled = false;
                engine.ExecutionCompleted -= Engine_ExecutionCompleted;
                engine.ExecutionAborted -= Engine_ExecutionAborted;
                engine.Stop();
                PrintLnStatement.Stop();
                engine = null;
                ButtonStart.IsEnabled = true;
                ButtonStop.IsEnabled = false;
            }
            
        }
        #endregion
        protected override void OnClosing(CancelEventArgs e)
        {
            if (Editor.IsModified)
            {
                var ret=MessageBox.Show(Properties.Resources.RequestSave, Properties.Resources.RequestSaveTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(ret== MessageBoxResult.Yes)
                {
                    OnSave(null, null);
                }
            }
            ScaleRatio = Editor.ScacleRatio;
            IniFile.WriteValue("Editor", "IsHighLight", ButtonHighligh.IsChecked + "");
            IniFile.WriteValue("Editor", "HighLightSpeed", (100-SliderSpeeder.Value) + "");
            int usedTime = 0;
            string v = IniFile.ReadValue("Editor", "UseCycle");
            if (!string.IsNullOrEmpty(v))
                int.TryParse(v, out usedTime);
            usedTime++;
            IniFile.WriteValue("Editor", "UseCycle", usedTime+"");
            string review = IniFile.ReadValue("Editor", "Reviewed");
            if(string.IsNullOrEmpty(review) && usedTime%10==4)
            {
            }
            try
            {
                engine?.Stop();
            }
            catch { }
            //Environment.Exit(0);
            base.OnClosing(e);
        }
        #region open, save, new, document 
        private void OnOpen(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".vsc";
            dlg.Filter = "Visual Script Files (*.vsc)|*.vsc|All Files (*.*)|*.*";
            dlg.Multiselect = false;
            dlg.InitialDirectory = LastFolder;
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                Editor.Script = null;
                Editor.IsEnabled = false;
                File = dlg.FileName;
                LastFolder = new FileInfo(dlg.FileName).DirectoryName;
                try
                {
                    Script script = Serialization.Load(File) as Script;
                    SetupScriptToolbar(script);
                    Editor.Script = script;
                    if (script == null)
                        return;
                    this.Title = Properties.Resources.VisualCodeEditor+" - "+File;
                    Editor.IsModified = false;
                    ShowMessage(toast.ShowSuccess, string.Format(Properties.Resources.SuccessLodFile, File));
                }
                catch(Exception ex)
                {
                    MessageBox.Show(string.Format(Properties.Resources.ExceptionLoadFile, File));
                    return;
                }
                Editor.IsEnabled = true;
                ButtonOpen.IsEnabled = true;
                ButtonSave.IsEnabled = true;
                ButtonStart.IsEnabled = true;
                ButtonSaveAs.IsEnabled = true;
                ButtonPrint.IsEnabled = true;
                ButtonStart.IsEnabled = true;
            }
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(File))
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.DefaultExt = ".vsc";
                dlg.Filter = "Visual Script Files (*.vsc)|*.vsc|All Files (*.*)|*.*";
                //dlg.Multiselect = false;
                if (!string.IsNullOrEmpty(LastFolder))
                    dlg.InitialDirectory = LastFolder;

                Nullable<bool> result = dlg.ShowDialog();
                if (result != true)
                {
                    return;
                }
                File = dlg.FileName;
                Title = Properties.Resources.VisualCodeEditor + " - " + File;
                LastFolder = new FileInfo(dlg.FileName).DirectoryName;
            }
            Serialization.Save((Script)Editor.Script, File);
            ShowMessage(toast.ShowSuccess, string.Format(Properties.Resources.SuccessLodFile, File));
            Editor.IsModified = false;
            ButtonOpen.IsEnabled = true;
            ButtonSave.IsEnabled = true;
            ButtonStart.IsEnabled = true;
            ButtonSaveAs.IsEnabled = true;
            ButtonPrint.IsEnabled = true;
        }

        private void OnPrint(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            Editor.Print();
        }

        private void OnSaveAs(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.DefaultExt = ".vsc";
            dlg.Filter = "Visual Script Files (*.vsc)|*.vsc|All Files (*.*)|*.*";
            //dlg.Multiselect = false;
            dlg.InitialDirectory = LastFolder;

            Nullable<bool> result = dlg.ShowDialog();
            if (result != true)
            {
                return;
            }
            File = dlg.FileName;
            LastFolder = new FileInfo(dlg.FileName).DirectoryName;
            Serialization.Save((Script)Editor.Script, File);
            Title = Properties.Resources.VisualCodeEditor + " - " + File;
            ShowMessage(toast.ShowSuccess, string.Format(Properties.Resources.SuccessLodFile, File));
            Editor.IsModified = false;
            ButtonOpen.IsEnabled = true;
            ButtonSave.IsEnabled = true;
            ButtonStart.IsEnabled = true;
            ButtonSaveAs.IsEnabled = true;
            ButtonPrint.IsEnabled = true;
        }
        private void OnNew(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            File = null;
            Title = "Visual Code Editor";
            using (var stream = GetType().Assembly.GetManifestResourceStream("ScratchNet.scripts.main.vsc"))
            {
                Script script = Serialization.Load(stream) as Script; 
                SetupScriptToolbar(script);
                Editor.Script = script;
                 stream.Close();
            }
            Editor.IsEnabled = true;
            ButtonOpen.IsEnabled = true;
            ButtonSave.IsEnabled = true;
            ButtonStart.IsEnabled = true;
            ButtonSaveAs.IsEnabled = true;
            ButtonPrint.IsEnabled = true;
            Editor.IsModified = true;
        }

        /// <summary>
        /// Ini configuration file name
        /// </summary>
        double _scaleRatio = 1;
        public double ScaleRatio
        {
            get
            {
                string ratioStr = IniFile.ReadValue("Editor", "ScaleRatio");
                if (string.IsNullOrEmpty(ratioStr))
                {
                    _scaleRatio = 1;
                    return 1;
                }
                try
                {
                    _scaleRatio = double.Parse(ratioStr);
                    return _scaleRatio;
                }
                catch { }
                _scaleRatio = 1;
                return 1;
            }
            set
            {
                IniFile.WriteValue("Editor", "ScaleRatio", value + "");
                _scaleRatio = value;
            }
        }
        string _currentFile;
        public string File
        {
            get
            {
                if (string.IsNullOrEmpty(_currentFile))
                    _currentFile = IniFile.ReadValue("Editor", "LastOpen");
                return _currentFile;
            }
            set
            {
                _currentFile = value;
                IniFile.WriteValue("Editor", "LastOpen", value);
            }
        }
        string _lastFolder = null;
        public string LastFolder
        {
            get
            {
                if (string.IsNullOrEmpty(_lastFolder))
                {
                    _lastFolder = IniFile.ReadValue("Editor", "LastFolder");
                }
                if (string.IsNullOrEmpty(_lastFolder))
                {
                    _lastFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "VisualCodeEditor");

                }
                return _lastFolder;
            }
            set
            {
                _lastFolder = value;
                IniFile.WriteValue("Editor", "LastFolder", value);
            }
        }
        #endregion
        private void OnClearError(object sender, RoutedEventArgs e)
        {
            ButtonClearError.IsEnabled = false;
            Editor.ClearHighlight();
        }

        private void OnHighlightClicked(object sender, RoutedEventArgs e)
        {
            IsHighlightStep = ButtonHighligh.IsChecked.Value;
        }

        private void OnSpeedValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //HighlightSpeed = 100-(int)SliderSpeeder.Value;
        }

        private void OnExample(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            ExampleWindow window = new ExampleWindow();
            window.Owner = this;
            bool? rt=window.ShowDialog();
            if (!rt.HasValue || !rt.Value)
                return;
            var c = window.Script;
            if (c == null)
                return;
            File = null;
            Title = Properties.Resources.VisualCodeEditor; 
            SetupScriptToolbar(c as Script);
            Editor.Script = c;
            Editor.IsEnabled = true;
            ButtonOpen.IsEnabled = true;
            ButtonSave.IsEnabled = true;
            ButtonStart.IsEnabled = true;
            ButtonSaveAs.IsEnabled = true;
            ButtonPrint.IsEnabled = true;
            Editor.IsModified = true;
        }

        private void OnHelp(object sender, RoutedEventArgs e)
        {
            VideoWindow v = new VideoWindow();
            v.Owner = this;
            v.Show();
        }

        private void OnSetting(object sender, RoutedEventArgs e)
        {
        }

        private void OnCopy(object sender, RoutedEventArgs e)
        {
            Editor.Copy();
        }

        private void OnPaste(object sender, RoutedEventArgs e)
        {
            Editor.Paste(new Point());
        }

        private void OnLanguage(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            ContextMenu menu = btn.ContextMenu;
            menu.Items.Clear();
            menu.Items.Add(CreateLanMenuItem(Properties.Resources.English, "en"));
            menu.Items.Add(CreateLanMenuItem(Properties.Resources.Chinese, "zh-CN"));
            btn.ContextMenu.IsOpen = true;
        }
        MenuItem CreateLanMenuItem(string text, string locale)
        {
            MenuItem mi = new MenuItem() { Header = text };
            mi.Tag = locale;
            mi.Click += SetLanguage;
            return mi;
        }

        private void SetLanguage(object sender, RoutedEventArgs e)
        {
            string locale = (sender as MenuItem).Tag as string;
            IniFile.WriteValue("Language", "Language", locale);
            MessageBox.Show(Properties.Resources.LanguageChange,"", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnAbout(object sender, RoutedEventArgs e)
        {
            AbountWindow about = new AbountWindow();
            about.Owner = this;
            about.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            about.ShowDialog();
        }
    }
}
