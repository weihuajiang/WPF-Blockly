using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScratchNet
{
    /// <summary>
    /// Interaction logic for ScriptPlayer.xaml
    /// </summary>
    public partial class ScriptPlayer : UserControl
    {
        public ScriptPlayer()
        {
            InitializeComponent();
            InputManager.Current.PreProcessInput += (sender, e) =>
            {
                if (e.StagingItem.Input is KeyEventArgs)
                {
                    OnKeyDownGlobal(sender,
                      (KeyEventArgs)e.StagingItem.Input);
                }
            };  
        }
        public void Clear()
        {
            controls.Clear();
            PlayScreen.Children.Clear();
        }
        Dictionary<Instance, FrameworkElement> controls = new Dictionary<Instance, FrameworkElement>();
        void Setup(Image img, Instance inst){
            Sprite sp = inst.Class as Sprite;
            BitmapFrame bFrame = sp.Images[sp.CurrentImage].Image as BitmapFrame;
            if(img.Source!=bFrame)
                img.Source = bFrame;
            double size = sp.Size;
            img.Width = bFrame.Width * size*PlayScreen.ActualWidth/CurrentEnviroment.ScreenWidth / 100;
            img.Height = bFrame.Height * size*PlayScreen.ActualHeight/CurrentEnviroment.ScreenHeight / 100;
            double x = sp.X * PlayScreen.ActualWidth / CurrentEnviroment.ScreenWidth - img.Width / 2;
            double y = sp.Y * PlayScreen.ActualHeight / CurrentEnviroment.ScreenHeight - img.Height / 2;
            img.RenderTransformOrigin = new Point(0.5, 0.5);
            img.RenderTransform = null;
            if (sp.RotationMode == CharacterRotationMode.Any)
            {
                RotateTransform rt = new RotateTransform();
                if (sp.RotationMode == CharacterRotationMode.Any)
                    rt.Angle = sp.Direction;
                img.RenderTransform = rt;
            }
            else if (sp.RotationMode == CharacterRotationMode.LeftRight)
            {
                ScaleTransform st = new ScaleTransform();
                if (sp.Direction >= 90 && sp.Direction<270)
                    st.ScaleX = -1;
                else
                    st.ScaleX = 1;
                st.ScaleY = 1;
                img.RenderTransform = st;
            }
            Canvas.SetLeft(img, x);
            Canvas.SetTop(img, y);
            Canvas.SetZIndex(img, sp.Layer);
            if (sp.Visible)
                img.Visibility = Visibility.Visible;
            else
                img.Visibility = Visibility.Hidden;
        }
        //Image backgroundImage = new Image();
        public void DrawScript()
        {
            lock (this)
            {
                /*
                backgroundImage.Source = CurrentEnviroment.Game.Background.Images[CurrentEnviroment.Game.Background.CurrentImage].Image;
                if (!PlayScreen.Children.Contains(backgroundImage))
                {
                    backgroundImage.Stretch = Stretch.Fill;
                    PlayScreen.Children.Add(backgroundImage);
                    Canvas.SetLeft(backgroundImage, 0);
                    Canvas.SetTop(backgroundImage, 0);
                }
                backgroundImage.Width = PlayScreen.ActualWidth;
                backgroundImage.Height = PlayScreen.ActualHeight;
                Canvas.SetBottom(backgroundImage, PlayScreen.ActualHeight);
                Canvas.SetRight(backgroundImage, PlayScreen.ActualWidth);
                 */
                foreach (Instance inst in CurrentEnviroment.Game.Instances.Keys)
                {
                    if (controls.ContainsKey(inst))
                    {
                        Image img = controls[inst] as Image;
                        Setup(img, inst);
                    }
                    else
                    {
                        Image img = new Image();
                        Setup(img, inst);
                        PlayScreen.Children.Add(img);
                        img.Tag = inst;
                        controls.Add(inst, img);
                    }
                }
            }
        }
        ExecutionEnvironment engine;
        Dictionary<Sprite, ExecutionEnvironment> allEnvs = new Dictionary<Sprite, ExecutionEnvironment>();
        private void OnRun(object sender, RoutedEventArgs e)
        {
            //ButtonStart.IsEnabled = false;
            //ButtonStop.IsEnabled = true;
            if(engine!=null)
            {
                ///engine.Stop();
               /// engine = null;
            }
            engine = new ExecutionEnvironment();
            PlayScreen.Children.Clear();
            controls.Clear();
            CurrentEnviroment.Game.Instances.Clear();
            allEnvs.Clear();
            engine.RegisterValue("$$Player", this);
            foreach (Sprite sp in CurrentEnviroment.Game.Sprites)
            {
                CurrentEnviroment.Game.Instances.Add(new Instance(sp), sp);
                ExecutionEnvironment playEnv = new ExecutionEnvironment(engine);
                allEnvs.Add(sp, playEnv);
                playEnv.RegisterValue("$$INSTANCE$$", sp);
                playEnv.Execute(sp);
                foreach(var f in sp.Handlers)
                {
                    if(f is StartEventHandler)
                    {
                        new Thread(() =>
                        {
                            f.Execute(playEnv);
                        }).Start();
                    }
                }
            }
        }

        private void OnStop(object sender, RoutedEventArgs e)
        {
            if (engine != null)
            {
                ///engine.Stop();
                ///engine = null;
            }
            //ButtonStart.IsEnabled = true;
            //ButtonStop.IsEnabled = false;
        }
        int current = 0;
        private void OnKeyDownGlobal(object sender, KeyEventArgs e)
        {
            if (current % 2 == 1 && e.IsDown)
            {
                foreach(var sp in allEnvs.Keys)
                {
                    foreach (var f in sp.Handlers)
                    {
                        if (f is KeyEventHandler)
                        {
                            var key = (int)e.Key;
                            if (key == ((KeyEventHandler)f).Key)
                            {
                                ExecutionEnvironment playEnv = allEnvs[sp];
                                new Thread(() =>
                                {
                                    f.Execute(playEnv);
                                }).Start();
                            }
                        }
                    }
                }

            }
            else
            {
                current = 0;
            }
            current++;
        }
        Point StartPoint;
        FrameworkElement DragObj;
        int dragLayer;
        private void OnMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            dragLayer = -1;
            StartPoint = e.GetPosition(PlayScreen);
            foreach (FrameworkElement child in PlayScreen.Children)
            {
                Point pt = e.GetPosition(child);
                if (child.Tag is Instance && new Rect(0, 0, child.ActualWidth, child.ActualHeight).Contains(pt))
                {
                    if (dragLayer <= Canvas.GetZIndex(child))
                    {
                        DragObj = child;
                        dragLayer = Canvas.GetZIndex(child);
                        if (engine != null)
                        {
                            Sprite sp = (DragObj.Tag as Instance).Class as Sprite;
                            foreach(var f in sp.Handlers)
                            {
                                if(f is ClickEventHandler)
                                {
                                    new Thread(() =>
                                    {
                                        f.Execute(allEnvs[sp]);
                                    }).Start();
                                }
                            }
                        }
                    }
                }
            }
        }

        private void OnMouseLeftUp(object sender, MouseButtonEventArgs e)
        {
            if (DragObj != null)
                DragObj.Effect = null;
            dragLayer = -1;
            DragObj = null;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(PlayScreen);
            Vector diff =   mousePos-StartPoint;
            if(e.LeftButton == MouseButtonState.Pressed && DragObj!=null)
            {
                Sprite sp = (DragObj.Tag as Instance).Class as Sprite;
                sp.X +=(int)(diff.X*CurrentEnviroment.ScreenWidth/PlayScreen.ActualWidth);
                sp.Y += (int)(diff.Y*CurrentEnviroment.ScreenHeight/PlayScreen.ActualHeight);
                Canvas.SetLeft(DragObj, Canvas.GetLeft(DragObj) + diff.X);
                Canvas.SetTop(DragObj, Canvas.GetTop(DragObj) + diff.Y);
                StartPoint = mousePos;
                DragObj.Effect = new DropShadowEffect() {ShadowDepth=6 };
            }
        }

        private void PlayScreen_MouseLeave(object sender, MouseEventArgs e)
        {
            if (DragObj != null)
                DragObj.Effect = null;
            dragLayer = -1;
            DragObj = null;
        }

        private void PlayScreen_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawScript();
        }

        private void OnPalyerKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void OnPlayerKeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        public bool IsFullSize = false;
        private void OnChangeSizeClicked(object sender, RoutedEventArgs e)
        {
            IsFullSize = !IsFullSize;
        }
    }
}
