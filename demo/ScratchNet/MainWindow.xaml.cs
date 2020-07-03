using Microsoft.Win32;
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
        public MainWindow()
        {
            AllocConsole();
            InitializeComponent();
            CurrentEnviroment.Game = new Game();
            this.ScriptEditor.IsEnabled = false;
            this.SpriteList.SpriteListView.SelectionChanged += SpriteListView_SelectionChanged;
            //this.SpriteList.BackgroundContainer.Checked += BackgroundContainer_Checked;
            this.ScriptPlayer.PlayScreen.PreviewMouseMove += PlayScreen_PreviewMouseMove;

            ScriptEditor.Register(((Color)ColorConverter.ConvertFromString("#4C97FF")), typeof(MoveStatement),
                typeof(RotationStatement), typeof(ReflectionStatement), typeof(SetRotationModeStatement), typeof(SetDegreeStatement),
                typeof(SetPositionStatement), typeof(SetPositionXStatement), typeof(SetPositionYStatement), typeof(TimedMoveStatement),
                typeof(XPositionExpression), typeof(YPositionExpression), typeof(DegreeExpression));

            ScriptEditor.Register(((Color)ColorConverter.ConvertFromString("#9966FF")), typeof(ResizeStatement),
                typeof(SetImageStatement), typeof(SetNextImageStatement), typeof(ShowStatement), typeof(HideStatement), typeof(SizeExpression));

            ScriptEditor.Register(((Color)ColorConverter.ConvertFromString("#FFBF00")), typeof(StartEventHandler),
                typeof(ClickEventHandler), typeof(MessageEvetHandler));

            List<ScriptStepGroup> toolbar = new List<ScriptStepGroup>();
            toolbar.Add(new ScriptStepGroup()
            {
                Name = Localize.GetString("xc_Motion"),
                Types = new List<object>(){
                    new WaitStatement(),
                    new MoveStatement(),
                    new RotationStatement(){ Direction=RotationDirection.Clockwise},
                    new RotationStatement(){ Direction=RotationDirection.CounterClockwise},
                    new ReflectionStatement(),
                    new SetRotationModeStatement(),
                    new SetDegreeStatement(),
                    new SetPositionStatement(),
                    new SetPositionXStatement(),
                    new SetPositionYStatement(),
                    new TimedMoveStatement(),
                    new XPositionExpression(),
                    new YPositionExpression(),
                    new DegreeExpression()
                }
            });
            toolbar.Add(new ScriptStepGroup()
            {
                Name = Localize.GetString("xc_Looks"),
                Types = new List<object>(){
                    new ResizeStatement(),
                    new SetImageStatement(),
                    new SetNextImageStatement(),
                    new ShowStatement(),
                    new HideStatement(),
                    new SizeExpression()
                }
            });
            toolbar.Add(new ScriptStepGroup()
            {
                Name = Localize.GetString("xc_Logic"),
                Types = new List<object>(){
                    new BinaryExpression(){ ValueType="boolean", Operator= Operator.Equal },//compare operator
                    new BinaryExpression(){ValueType="boolean", Operator= Operator.And },//logical operator
                    new NotExpression()
                }
            });
            toolbar.Add(new ScriptStepGroup()
            {
                Name = Localize.GetString("xc_Operation"),
                Types = new List<object>(){
                    new BinaryExpression(),
                    new ConditionalExpression(),
                    new RandomExpression()
                }
            });
            toolbar.Add(new ScriptStepGroup()
            {
                Name = Localize.GetString("xc_Control"),
                Types = new List<object>(){
                    new IfStatement(),
                    new IfStatement(){Alternate=new BlockStatement()},
                    new LoopStatement(),
                    new WhileStatement(),
                    new BreakStatement(),
                    new ContinueStatement(),
                    new ReturnStatement(),
                    new LogStatement(),
                    new TryStatement()
                }
            });
            toolbar.Add(new ScriptStepGroup()
            {
                Name = Localize.GetString("xc_Event"),
                Types = new List<object>(){
                    new StartEventHandler(),
                    new KeyEventHandler(),
                    new ClickEventHandler(),
                    new MessageEvetHandler()
                }
            });
            toolbar.Add(new ScriptStepGroup()
            {
                Name = Localize.GetString("xc_Variable"),
                Types = new List<object>(){
                    "CreateVariable",
                    new ExpressionStatement(){ Expression= new AssignmentExpression() }
                }
            });
            toolbar.Add(new ScriptStepGroup()
            {
                Name = Localize.GetString("xc_Method"),
                Types = new List<object>(){
                    "CreateFunction"
                }
            });
            ScriptEditor.SetToolBar(toolbar);
        }
        /*
        void BackgroundContainer_Checked(object sender, RoutedEventArgs e)
        {
            this.SpriteList.SpriteListView.SelectedValue = null;
            this.ScriptTab.Visibility = Visibility.Collapsed;
            this.ScriptEditor.Script = null;
            this.ScriptEditor.IsEnabled = false;
            ScriptEditorTab.SelectedIndex = 1;
            this.ResourceEditor.IsSprite = false;
            this.ResourceEditor.Resources = CurrentEnviroment.Game.Background.Images;
        }*/

        void PlayScreen_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point pt = e.GetPosition(this.ScriptPlayer.PlayScreen);
            PositionX.Text =(int)( pt.X*CurrentEnviroment.ScreenWidth/this.ScriptPlayer.PlayScreen.ActualWidth) + "";
            PositionY.Text = (int)(pt.Y * CurrentEnviroment.ScreenHeight / this.ScriptPlayer.PlayScreen.ActualHeight) + "";

        }

        void SpriteListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.ScriptEditor.IsEnabled = true;
            //this.SpriteList.BackgroundContainer.IsChecked = false;
            Sprite sp=this.SpriteList.SpriteListView.SelectedValue as Sprite;
            if (sp == null || SpriteList.SpriteListView.SelectedIndex<0)
                return;
            CurrentEnviroment.CurrentSpriteImages.Clear();
            foreach (Resource r in sp.Images)
            {
                CurrentEnviroment.CurrentSpriteImages.Add(r.DisplayName);
            }
            ScriptEditor.Script = sp;
            this.ScriptTab.Visibility = Visibility.Visible;
            if (this.SpriteList.SpriteListView.SelectedValue as Sprite == null)
                this.ScriptEditor.IsEnabled = false;
            else
            {
                this.ResourceEditor.IsSprite = true;
                this.ResourceEditor.Resources = CurrentEnviroment.Game.Sprites[SpriteList.SpriteListView.SelectedIndex].Images;
            }
            ScriptPlayer.DrawScript();
        }
        private void OnLoadFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".xml";
            dlg.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
            dlg.Multiselect = false;
            dlg.InitialDirectory = System.IO.Directory.GetCurrentDirectory();

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                ScriptEditor.Script = null;
                CurrentEnviroment.Game.Clear();
                ScriptPlayer.Clear();
                Serialization.Load(CurrentEnviroment.Game, dlg.FileName);
                this.SpriteList.SpriteListView.ItemsSource = CurrentEnviroment.Game.Sprites;
                if (CurrentEnviroment.Game.Sprites.Count > 0)
                {
                    this.SpriteList.SpriteListView.SelectedIndex = 0;
                    this.ResourceEditor.Resources = CurrentEnviroment.Game.Sprites[0].Images;
                }
                /*
                foreach (Resource r in CurrentEnviroment.Game.Background.Images)
                {
                    CurrentEnviroment.BackgroudImages.Add(r.DisplayName);
                }*/
                ScriptPlayer.DrawScript();
            }
        }
        private void OnSaveFile(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.DefaultExt = ".xml";
            dlg.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
            //dlg.Multiselect = false;
            dlg.InitialDirectory = System.IO.Directory.GetCurrentDirectory();

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                //Serialization.Save(ScriptEditor.Sprite, dlg.FileName);
                Serialization.Save(CurrentEnviroment.Game, dlg.FileName);
            }
        }
       
    }
}
