using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ScratchNet
{
    public interface IErrorHighlight
    {
        void HighlightError();
        void ClearHighlightError();
    }
    public class HighlightHelper
    {
        static Brush c;
        static bool hightLight = true;
        static int maxHightTime = 30;
        static Task hightTask;
        static Control currentControl;
        public static void HighlightError(Control control)
        {
            if(hightTask!=null && !hightTask.IsCompleted)
            {
                hightLight = false;
                hightTask.Wait();
            }
            c = control.Background;
            hightLight = true;
            currentControl = control;
            hightTask=Task.Run(() =>
            {
                DateTime timeout = DateTime.Now.AddSeconds(maxHightTime);
                while (hightLight)
                {
                    
                    Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        currentControl.Background = new SolidColorBrush(Colors.Red);
                    });
                    Thread.Sleep(300);

                    Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        currentControl.Background = c;
                    });
                    Thread.Sleep(500);
                    if (DateTime.Now > timeout)
                        return;
                }
            });
        }

        public static void ClearHighlightError()
        {
            hightLight = false;
        }
    }
}
