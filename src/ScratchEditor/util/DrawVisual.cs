using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScratchNet
{
    public class DrawVisualUtil
    {
        public static ImageSource VisualToImageSource(UIElement element)
        {
            //Point dpi = GetSystemDpi();

            var dpi = VisualTreeHelper.GetDpi(element);
            var bounds = VisualTreeHelper.GetDescendantBounds(element);
            var visual = new DrawingVisual();

            using (var dc = visual.RenderOpen())
            {
                dc.DrawRectangle(new VisualBrush(element), null, bounds);// new Rect(0, 0, bounds.Width, bounds.Height));
            }
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)(bounds.Width * dpi.DpiScaleX), (int)(bounds.Height * dpi.DpiScaleY), dpi.PixelsPerInchX, dpi.PixelsPerInchY, PixelFormats.Default);
            //rtb.Render(element);
            rtb.Render(visual);
            rtb.Freeze();
            return rtb;

        }
    }
}
