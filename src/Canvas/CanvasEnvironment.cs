using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ScratchNet
{
    class CanvasEnvironment
    {
        static string CanvasName = "&&&&&_Canvas++++++++++++++++";
        public static DrawWindow GetCanvas(ExecutionEnvironment environment)
        {
            DrawWindow canvas = null;
            if (!environment.HasValue(CanvasName))
            {
                ExecutionEnvironment b = environment.GetBaseEnvironment();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    canvas = new DrawWindow();
                    if (Application.Current.MainWindow != null)
                        canvas.Owner = Application.Current.MainWindow;
                    canvas.Show();
                });
                b.RegisterValue(CanvasName, canvas);
            }
            else
                canvas = environment.GetValue(CanvasName) as DrawWindow;
            return canvas;
        }
    }
}
