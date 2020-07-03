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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ScratchNet
{
    /// <summary>
    /// Interaction logic for DrawWindow.xaml
    /// </summary>
    public partial class DrawWindow : Window
    {
        public DrawWindow()
        {
            InitializeComponent();
        }
        public Color Color
        {
            get
            {
                if (Thread.CurrentThread.ManagedThreadId == Dispatcher.Thread.ManagedThreadId)
                {
                    return CanvasDraw.Color;
                }
                else
                {
                    Color value = new Color();
                    Dispatcher.Invoke(() =>
                    {
                        value = CanvasDraw.Color;
                    });
                    return value;
                }
            }
            set
            {
                if (Thread.CurrentThread.ManagedThreadId == Dispatcher.Thread.ManagedThreadId)
                {
                    CanvasDraw.Color = value;
                }
                else
                {
                    Dispatcher.Invoke(() =>
                    {
                        CanvasDraw.Color = value;
                    });
                }
            }
        }
        public Color Fill
        {
            get
            {
                if (Thread.CurrentThread.ManagedThreadId == Dispatcher.Thread.ManagedThreadId)
                {
                    return CanvasDraw.Fill;
                }
                else
                {
                    Color value=new Color();
                    Dispatcher.Invoke(() =>
                    {
                        value = CanvasDraw.Fill;
                    });
                    return value;
                }
            }
            set
            {
                if (Thread.CurrentThread.ManagedThreadId == Dispatcher.Thread.ManagedThreadId)
                {
                    CanvasDraw.Fill = value;
                }
                else
                {
                    Dispatcher.Invoke(() =>
                    {
                        CanvasDraw.Fill = value;
                    });
                }
            }
        }
        public double Thickness
        {
            get
            {
                if (Thread.CurrentThread.ManagedThreadId == Dispatcher.Thread.ManagedThreadId)
                {
                    return CanvasDraw.Thickness;
                }
                else
                {
                    double value = 0;
                    Dispatcher.Invoke(() =>
                    {
                        value = CanvasDraw.Thickness;
                    });
                    return value;
                }
            }
            set
            {
                if (Thread.CurrentThread.ManagedThreadId == Dispatcher.Thread.ManagedThreadId)
                {
                    CanvasDraw.Thickness = value;
                }
                else
                {
                    Dispatcher.Invoke(() =>
                    {
                        CanvasDraw.Thickness = value;
                    });
                }
            }
        }
        public double Heading
        {
            get
            {
                if (Thread.CurrentThread.ManagedThreadId == Dispatcher.Thread.ManagedThreadId)
                {
                    return CanvasDraw.Heading;
                }
                else
                {
                    double value = 0;
                    Dispatcher.Invoke(() =>
                    {
                        value = CanvasDraw.Heading;
                    });
                    return value;
                }
            }
            set
            {
                if (Thread.CurrentThread.ManagedThreadId == Dispatcher.Thread.ManagedThreadId)
                {
                    CanvasDraw.Heading = value;
                }
                else
                {
                    Dispatcher.Invoke(() =>
                    {
                        CanvasDraw.Heading = value;
                    });
                }
            }
        }
        public bool IsFill
        {
            get
            {
                if (Thread.CurrentThread.ManagedThreadId == Dispatcher.Thread.ManagedThreadId)
                {
                    return CanvasDraw.IsFill;
                }
                else
                {
                    bool value = false;
                    Dispatcher.Invoke(() =>
                    {
                        value = CanvasDraw.IsFill;
                    });
                    return value;
                }
            }
        }
        public bool IsPenDown
        {
            get
            {
                if (Thread.CurrentThread.ManagedThreadId == Dispatcher.Thread.ManagedThreadId)
                {
                    return CanvasDraw.IsPenDown;
                }
                else
                {
                    bool value = false;
                    Dispatcher.Invoke(() =>
                    {
                        value = CanvasDraw.IsPenDown;
                    });
                    return value;
                }
            }
        }
        public double X
        {
            get
            {
                if (Thread.CurrentThread.ManagedThreadId == Dispatcher.Thread.ManagedThreadId)
                {
                    return CanvasDraw.X;
                }
                else
                {
                    double value = 0;
                    Dispatcher.Invoke(() =>
                    {
                        value = CanvasDraw.X;
                    });
                    return value;
                }
            }
            set
            {
                if (Thread.CurrentThread.ManagedThreadId == Dispatcher.Thread.ManagedThreadId)
                {
                    CanvasDraw.X = value;
                }
                else
                {
                    Dispatcher.Invoke(() =>
                    {
                        CanvasDraw.X = value;
                    });
                }
            }
        }
        public double Y
        {
            get
            {
                if (Thread.CurrentThread.ManagedThreadId == Dispatcher.Thread.ManagedThreadId)
                {
                    return CanvasDraw.Y;
                }
                else
                {
                    double value = 0;
                    Dispatcher.Invoke(() =>
                    {
                        value=CanvasDraw.Y ;
                    });
                    return value;
                }
            }
            set
            {
                if (Thread.CurrentThread.ManagedThreadId == Dispatcher.Thread.ManagedThreadId)
                {
                    CanvasDraw.Y = value;
                }
                else
                {
                    Dispatcher.Invoke(() =>
                    {
                        CanvasDraw.Y = value;
                    });
                }
            }
        }
        public double TextSize {

            get
            {
                if (Thread.CurrentThread.ManagedThreadId == Dispatcher.Thread.ManagedThreadId)
                {
                    return CanvasDraw.FontSize;
                }
                else
                {
                    double value = 0;
                    Dispatcher.Invoke(() =>
                    {
                        value = CanvasDraw.FontSize;
                    });
                    return value;
                }
            }
            set
            {
                if (Thread.CurrentThread.ManagedThreadId == Dispatcher.Thread.ManagedThreadId)
                {
                    CanvasDraw.FontSize = value;
                }
                else
                {
                    Dispatcher.Invoke(() =>
                    {
                        CanvasDraw.FontSize = value;
                    });
                }
            }
        }
        public Typeface Font
        {
            get
            {
                if (Thread.CurrentThread.ManagedThreadId == Dispatcher.Thread.ManagedThreadId)
                {
                    return CanvasDraw.Font;
                }
                else
                {
                    Typeface value = null;
                    Dispatcher.Invoke(() =>
                    {
                        value = CanvasDraw.Font;
                    });
                    return value;
                }
            }
            set
            {
                if (Thread.CurrentThread.ManagedThreadId == Dispatcher.Thread.ManagedThreadId)
                {
                    CanvasDraw.Font = value;
                }
                else
                {
                    Dispatcher.Invoke(() =>
                    {
                        CanvasDraw.Font = value;
                    });
                }
            }
        }
        public void StartFill()
        {
            if (Thread.CurrentThread.ManagedThreadId == Dispatcher.Thread.ManagedThreadId)
            {
                CanvasDraw.StartFill();
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    CanvasDraw.StartFill();
                });
            }
        }
        public void StopFill()
        {
            if (Thread.CurrentThread.ManagedThreadId == Dispatcher.Thread.ManagedThreadId)
            {
                CanvasDraw.StopFill();
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    CanvasDraw.StopFill();
                });
            }
        }
        public void PenUp()
        {
            if (Thread.CurrentThread.ManagedThreadId == Dispatcher.Thread.ManagedThreadId)
            {
                CanvasDraw.PenUp();
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    CanvasDraw.PenUp();
                });
            }
        }
        public void PenDown()
        {
            if (Thread.CurrentThread.ManagedThreadId == Dispatcher.Thread.ManagedThreadId)
            {
                CanvasDraw.PenDown();
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    CanvasDraw.PenDown();
                });
            }
        }
        public void Text(string text)
        {
            if (Thread.CurrentThread.ManagedThreadId == Dispatcher.Thread.ManagedThreadId)
            {
                CanvasDraw.Text(text);
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    CanvasDraw.Text(text);
                });
            }
        }
        public void Arc(double step, double angle, double xRadius, double yRadius)
        {
            if (Thread.CurrentThread.ManagedThreadId == Dispatcher.Thread.ManagedThreadId)
            {
                CanvasDraw.Arc(step, angle, xRadius, yRadius);
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    CanvasDraw.Arc(step, angle, xRadius, yRadius);
                });
            }
        }
        public void LineTo(double x, double y)
        {
            if (Thread.CurrentThread.ManagedThreadId == Dispatcher.Thread.ManagedThreadId)
            {
                CanvasDraw.LineTo(x, y);
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    CanvasDraw.LineTo(x, y);
                });
            }
        }
        public void Line(double step)
        {
            if (Thread.CurrentThread.ManagedThreadId == Dispatcher.Thread.ManagedThreadId)
            {
                CanvasDraw.Line(step);
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    CanvasDraw.Line(step);
                });
            }
        }
        public void Goto(double x, double y)
        {
            if (Thread.CurrentThread.ManagedThreadId == Dispatcher.Thread.ManagedThreadId)
            {
                CanvasDraw.Goto(x, y);
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    CanvasDraw.Goto(x, y);
                });
            }
        }
        public void Turn(double degree)
        {
            if (Thread.CurrentThread.ManagedThreadId == Dispatcher.Thread.ManagedThreadId)
            {
                CanvasDraw.Turn(degree);
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    CanvasDraw.Turn(degree);
                });
            }
        }
        public void Clear()
        {
            if (Thread.CurrentThread.ManagedThreadId == Dispatcher.Thread.ManagedThreadId)
            {
                CanvasDraw.Clear();
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    CanvasDraw.Clear();
                });
            }
        }
        public void Reset()
        {
            if (Thread.CurrentThread.ManagedThreadId == Dispatcher.Thread.ManagedThreadId)
            {
                CanvasDraw.Reset();
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    CanvasDraw.Reset();
                });
            }
        }

        private void OnCanvasMouseMove(object sender, MouseEventArgs e)
        {
            var p = e.GetPosition(CanvasDraw);
            StatusLabel.Content = "x="+p.X + " , " + "y="+ p.Y;
        }
    }
}
