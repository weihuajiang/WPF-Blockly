using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ScratchNet
{
    public enum DrawType
    {
        Geometry,
        Text
    }
    public class DrawText
    {
        public double Angle { get; set; }
        public FormattedText Text { get; set; }
        public Point Position { get; set; }
    }
    public class DrawGeometry
    {
        public DrawGeometry(Geometry geometry, Pen pen)
        {
            Geometry = geometry;
            Pen = pen;
            Type = DrawType.Geometry;
        }
        public DrawGeometry(DrawText text)
        {
            Text = text;
            Type = DrawType.Text;
        }
        public DrawText Text { get; set; }
        public DrawType Type { get; set; } = DrawType.Geometry;
        public Geometry Geometry { get;private set; }
        public Pen Pen { get; private set; }
    }
    public class DrawShape
    {
        public List<DrawGeometry> Shape { get; set; } = new List<DrawGeometry>();
        public Brush Brush { get; set; } = null;
        public PathGeometry Path { get; set; } = new PathGeometry();
    }
    public class DrawCanvas : Canvas
    {
        public DrawCanvas()
        {
            currentFigure = new PathFigure() { StartPoint = new Point(X, Y)};
            Background = new SolidColorBrush(Colors.White);
        }

        public Color Color { get; set; } = Colors.Black;
        public Color Fill { get; set; } = Colors.Green;
        public double Thickness { get; set; } = 1;
        public bool IsFill { get; private set; } = false;
        public bool IsPenDown { get; private set; } = true;
        public double Heading { get; set; } = 0;
        public double FontSize { get; set; } = 12;
        public Typeface Font { get; set; } = new Typeface("Segoe UI");

        public double X { get; set; }
        public double Y { get; set; } = 0;

        //
        List<DrawShape> shapes = new List<DrawShape>();
        List<DrawGeometry> currentShape =new List<DrawGeometry>();
        PathFigure currentFigure ;
        public void StartFill()
        {
            if (IsPenDown && !IsFill)
            {
                Wait();
                StopCurrentShape();
                currentShape = new List<DrawGeometry>();
                currentFigure = new PathFigure() { StartPoint = new Point(X, Y) };
                Unlock();
                InvalidateVisual();
            }
            IsFill = true;
        }
        public void StopFill()
        {
            if (IsPenDown && IsFill)
            {
                Wait();
                StopCurrentShape();
                currentShape = new List<DrawGeometry>();
                currentFigure = new PathFigure() { StartPoint = new Point(X, Y) };
                Unlock();
                InvalidateVisual();
            }
            IsFill = false;
        }
        public void PenUp()
        {
            if (!IsPenDown)
                return;
            IsPenDown = false;

            Wait();
            StopCurrentShape();
            Unlock();
            InvalidateVisual();
        }
        void StopCurrentShape()
        {
            if (currentShape != null && currentShape.Count>0)
            {
                var s = new DrawShape();
                s.Shape = currentShape;
                if (IsFill)
                {
                    s.Brush = new SolidColorBrush(Fill);
                    s.Path.Figures.Add(currentFigure);
                }
                shapes.Add(s);
                currentShape = null;
                currentFigure = null;
            }
            else
            {
                currentShape = null;
                currentFigure = null;
            }
        }
        public void PenDown()
        {
            if (!IsPenDown)
            {
                IsPenDown = true;
                if (currentShape == null)
                {
                    currentShape = new List<DrawGeometry>();
                    currentFigure = new PathFigure() { StartPoint = new Point(X, Y) };
                }
            }
        }
        public void Text(string text)
        {
            if (IsPenDown)
            {
                Wait();
                FormattedText ft = new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, Font, FontSize, new SolidColorBrush(Color.FromRgb(Color.R, Color.G, Color.B)), VisualTreeHelper.GetDpi(this).PixelsPerDip);
                DrawText dt = new DrawText();
                dt.Angle = Heading;
                dt.Text = ft;
                dt.Position = new Point(X, Y);
                currentShape.Add(new DrawGeometry(dt));
                Unlock();
                InvalidateVisual();
            }
        }
        public void Arc(double step, double angle, double xRadius, double yRadius)
        {
            if (IsPenDown)
            {
                Wait();
                double x = X + step * Math.Cos(Math.PI * Heading / 180);
                double y = Y + step * Math.Sin(Math.PI * Heading / 180);
                PathFigure arc = new PathFigure();
                arc.StartPoint = new Point(X, Y);
                arc.Segments.Add(new ArcSegment(new Point(x, y), new Size(xRadius, yRadius), Math.Abs(angle), Math.Abs(angle) > 180, angle >= 0 ? SweepDirection.Clockwise : SweepDirection.Counterclockwise, true));
                PathGeometry line = new PathGeometry();
                line.Figures.Add(arc);
                currentShape.Add(new DrawGeometry(line, new Pen(new SolidColorBrush(Color.FromRgb(Color.R, Color.G, Color.B)), Thickness)));
                currentFigure.Segments.Add(new ArcSegment(new Point(x, y), new Size(xRadius, yRadius), Math.Abs(angle), Math.Abs(angle) > 180, angle >= 0 ? SweepDirection.Clockwise : SweepDirection.Counterclockwise, true));
                X = x;
                Y = y;
                Unlock();
                InvalidateVisual();
            }
            else
            {
                double x = X + step * Math.Cos(Math.PI * Heading / 180);
                double y = Y + step * Math.Sin(Math.PI * Heading / 180);
                X = x;
                Y = y;
            }
        }
        public void LineTo(double x, double y)
        {
            if (IsPenDown)
            {
                Wait();
                LineGeometry line = new LineGeometry();
                line.StartPoint = new Point(X, Y);
                line.EndPoint = new Point(x, y);
                currentShape.Add(new DrawGeometry(line, new Pen(new SolidColorBrush(Color.FromRgb(Color.R, Color.G, Color.B)), Thickness)));
                currentFigure.Segments.Add(new LineSegment(new Point(x, y), true));
                X = x;
                Y = y;
                Unlock();
                InvalidateVisual();
            }
            else
            {
                X = x;
                Y = y;
            }
        }
        public void Line(double step)
        {
            if (IsPenDown)
            {
                Wait();
                double x = X + step * Math.Cos(Math.PI * Heading / 180);
                double y = Y + step * Math.Sin(Math.PI * Heading / 180);
                LineGeometry line = new LineGeometry();
                line.StartPoint = new Point(X, Y);
                line.EndPoint = new Point(x, y);
                currentShape.Add(new DrawGeometry(line, new Pen(new SolidColorBrush(Color.FromRgb(Color.R, Color.G, Color.B)), Thickness)));
                currentFigure.Segments.Add(new LineSegment(new Point(x, y), true));
                X = x;
                Y = y;
                Unlock();
                InvalidateVisual();
            }
            else
            {
                double x = X + step * Math.Cos(Math.PI * Heading / 180);
                double y = Y + step * Math.Sin(Math.PI * Heading / 180);
                X = x;
                Y = y;
            }
        }
        public void Goto(double x, double y)
        {
            if (IsPenDown)
            {
                Wait();
                StopCurrentShape();
                X = x;
                Y = y;
                currentShape = new List<DrawGeometry>();
                currentFigure = new PathFigure() { StartPoint = new Point(X, Y) };
                Unlock();
                InvalidateVisual();
            }
            else
            {
                X = x;
                Y = y;
            }
            
        }
        public void Turn(double degree)
        {
            Heading += degree;
            while (Heading < 0)
            {
                Heading += 360;
            }
            while (Heading > 360)
            {
                Heading -= 360;
            }
        }
        public void Clear()
        {
            Wait();
            shapes?.Clear();
            currentShape?.Clear();
            currentFigure = null;
            currentFigure = new PathFigure() { StartPoint = new Point(X, Y) };
            Unlock();
            InvalidateVisual();
        }
        public void Reset()
        {
            Wait();
            shapes?.Clear();
            currentShape?.Clear();
            currentFigure = null;
            Color = Colors.Black;
            Fill = Colors.Green;
            Thickness = 1;
            IsFill = false;
            IsPenDown = true;
            Heading = 0;
            X = 0;
            Y = 0;
            currentFigure = new PathFigure() { StartPoint = new Point(X, Y) };
            Unlock();
            InvalidateVisual();
        }
        AutoResetEvent mutext = new AutoResetEvent(true);
        bool Wait(int milliSeconds)
        {
            return mutext.WaitOne(milliSeconds);
        }
        bool Wait()
        {
            return mutext.WaitOne();
        }
        void Unlock()
        {
            mutext.Set();
        }
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            Wait();
            foreach(var s in shapes)
            {
                if (s.Brush != null)
                {
                    dc.DrawGeometry(s.Brush, null, s.Path);
                }
                foreach (var g in s.Shape)
                {
                    if(g.Type== DrawType.Geometry)
                        dc.DrawGeometry(null, g.Pen, g.Geometry);
                    else
                    {
                        if (g.Text.Angle != 0)
                        {
                            var t = new RotateTransform(g.Text.Angle, g.Text.Position.X, g.Text.Position.Y);
                            dc.PushTransform(t);
                        }
                        dc.DrawText(g.Text.Text, g.Text.Position);
                        if (g.Text.Angle != 0)
                            dc.Pop();
                    }
                }
            }
            if (currentShape != null)
            {
                if (IsFill)
                {
                    PathGeometry path = new PathGeometry();
                    path.Figures.Add(currentFigure);
                    dc.DrawGeometry(new SolidColorBrush(Fill), null, path);
                }
                foreach (var g in currentShape)
                {
                    if(g.Type== DrawType.Geometry)
                        dc.DrawGeometry(null, g.Pen, g.Geometry);
                    else
                    {
                        if (g.Text.Angle != 0)
                        {
                            var t = new RotateTransform(g.Text.Angle, g.Text.Position.X, g.Text.Position.Y);
                            dc.PushTransform(t);
                        }
                        dc.DrawText(g.Text.Text, g.Text.Position);
                        if (g.Text.Angle != 0)
                            dc.Pop();
                    }
                }
            }
            Unlock();
        }
    }
}
