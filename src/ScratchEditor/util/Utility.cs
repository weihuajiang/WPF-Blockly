using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace ScratchNet
{
    class Utility
    {

        public static Node FindParents(Class script, Node node)
        {
            foreach (var f in script.Functions)
            {
                if (f.Equals(node))
                    return f;
                var p = FindParentsNode(f, node);
                if (p != null)
                    return p;
            }

            return null;
        }
        public static Node FindParentsNode(Node container, Node node)
        {
            PropertyInfo[] pinfo = container.GetType().GetProperties();
            foreach(var p in pinfo)
            {
                if (!p.CanWrite || !p.CanRead)
                    continue;
                var value = p.GetValue(container);
                if(value is Node)
                {
                    if (value.Equals(node))
                        return container;
                    var f = FindParentsNode(value as Node, node);
                    if (f != null)
                        return f;
                }
                else if(value is List<Expression>)
                {
                    var list = value as List<Expression>;
                    foreach(var e in list)
                    {
                        if (e.Equals(node))
                            return container;
                        else
                        {
                            var f = FindParentsNode(e, node);
                            if (f != null)
                                return f;
                        }
                    }
                       
                }
                if (value is BlockStatement)
                {
                    foreach (var px in ((BlockStatement)value).Body)
                    {
                        if (px.Equals(node))
                            return container;
                        var f = FindParentsNode(px as Node, node);
                        if (f != null)
                            return f;
                    }
                }
            }
            return null;
        }
        public static Control FindControl(FrameworkElement depObj, Node node)
        {
            List<FrameworkElement> elements = new List<FrameworkElement>();
            elements.Add(depObj as FrameworkElement);
            do
            {
                FrameworkElement current = elements[0];
                elements.RemoveAt(0);
                if (current == null)
                {
                    continue;
                }
                /*
                if(current is TextBox)
                {
                    if (current.Visibility != Visibility.Visible)
                        continue;
                     TextBoxExpressionHolder txt = current.DataContext as TextBoxExpressionHolder;
                    if (txt != null)
                    {
                        ExpressionDescriptor d = txt.ExpressionDescriptor;
                        if (d != null)
                        {
                            try
                            {
                                var m = d.Source.GetType().GetProperty(d.Name).GetValue(d.Source);
                                if (node.Equals(m))
                                    return current as Control;
                            }
                            catch { }
                        }
                    }
                }
                */
                if(current is TextBoxExpressionHolder)
                {
                    if (current.Visibility != Visibility.Visible)
                        continue;
                    TextBoxExpressionHolder txt = current as TextBoxExpressionHolder;
                    if (txt != null)
                    {
                        ExpressionDescriptor d = txt.ExpressionDescriptor;
                        if (d != null)
                        {
                            try
                            {
                                var m = d.Source.GetType().GetProperty(d.Name).GetValue(d.Source);
                                if (node.Equals(m))
                                    return current as Control;
                            }
                            catch { }
                        }
                    }
                }
                if(current is FunctionControl)
                {
                    Node value = (current as FunctionControl).Function;
                    if (node.Equals(value))
                        return current as Control;
                }
                else if(current is StatementControl)
                {
                    Node value = (current as StatementControl).Statement;
                    if (node.Equals(value))
                        return current as Control;
                }
                else if(current is ExpressionControl)
                {
                    Node value = (current as ExpressionControl).Expression;
                    if (node.Equals(value))
                        return current as Control;
                }

                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(current); i++)
                {
                    FrameworkElement child = VisualTreeHelper.GetChild(current, i) as FrameworkElement;
                        elements.Add(child);
                }
            } while (elements.Count > 0);
            return null;
        }
        public static T GetChildAtPoint<T>(DependencyObject depObj, Point pt, 
            out Rect region,
            DependencyObject excludObject = null)
                where T : DependencyObject
        {
            List<FrameworkElement> elements = new List<FrameworkElement>();
            FrameworkElement current=depObj as FrameworkElement;
            region = new Rect(); ;
            int currentIndex = -1;
            double width = 0;
            double height = 0;
            double distance = 6;
            double top = 0;
            do
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(current); i++)
                {
                    FrameworkElement child = VisualTreeHelper.GetChild(current, i) as FrameworkElement;
                    if (child is Control)
                    {
                        try
                        {
                            width = 0;
                            height = 0;
                            top = 0;
                            height = 0;
                            width = (child as Control).ActualWidth;
                            height = (child as Control).ActualHeight;
                            if (typeof(T) == typeof(BlockIndicator))
                            {
                                top = (-1) * distance;
                                height += 2 * distance;
                            }
                        }
                        catch (Exception e) { }
                        Point point = ((Control)child).PointFromScreen(pt);
                        if (width > 0 && height > 0 && !(new Rect(0, top, width, height)).Contains(point))
                        {
                            continue;
                        }
                        else
                        {
                            elements.Add(child);
                        }
                    }
                    else if(child!=null)
                    {
                        elements.Add(child);
                    }
                    else
                    {
                    }
                }
                currentIndex++;
                current = null;
                if (elements.Count==0 || currentIndex >= elements.Count)
                    currentIndex = -1;
                else
                    current = elements[currentIndex];
            }
            while (currentIndex >= 0 && current!=null);
            for (int i = elements.Count - 1; i >= 0; i--)
            {
                FrameworkElement child = elements[i];
                if (child is T && child!=excludObject)
                {
                    Point point = ((Visual)child).PointFromScreen(pt);
                    double actualWidth = (child as Control).ActualWidth;
                    double actualHeight = (child as Control).ActualHeight;
                    if (child is ActualSizeAdjustment)
                    {
                        double w2 = 0;
                        double h2 = 0;
                        (child as ActualSizeAdjustment).GetActualSize(out w2, out h2);
                        if (w2 > 0)
                            actualWidth = w2;
                        if (h2 > 0)
                            actualHeight = h2;
                    }
                    Rect rect = new Rect(0, 0, actualWidth, actualHeight);
                    if (typeof(T) == typeof(BlockIndicator))
                    {
                        rect = new Rect(0, -distance, actualWidth,
                            actualHeight + 2 * distance);
                    }
                    if (rect.Contains(point))
                    {
                        elements.Clear();
                        region = rect;
                        return child as T;
                    }

                }
            }
            elements.Clear();
             return null;
        }
        public static bool ContainsChilds<T, K>(DependencyObject obj) where T : DependencyObject
        {
            List<DependencyObject> list = new List<DependencyObject>();
            int currentIndex = -1;
            DependencyObject current = obj;
            do
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(current); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(current, i);
                    list.Add(child);
                }
                currentIndex++;
                if (currentIndex < 0 || currentIndex >= list.Count)
                {
                    currentIndex = -1;
                }
                else
                    current = list[currentIndex];
            }
            while (currentIndex >= 0 && current != null);
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i] is T || list[i] is K)
                {
                    list.Clear();
                    return true;
                }
            }
            list.Clear();
            return false;
        }
        public static bool ContainsChild<T>(DependencyObject obj) where T : DependencyObject
        {
            List<DependencyObject> list = new List<DependencyObject>();
            int currentIndex = -1;
            DependencyObject current = obj;
            do
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(current); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(current, i);
                    list.Add(child);
                }
                currentIndex++;
                if (currentIndex < 0 || currentIndex >= list.Count)
                {
                    currentIndex = -1;
                }
                else
                    current = list[currentIndex];
            }
            while (currentIndex >= 0 && current != null);
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i] is T)
                {
                    list.Clear();
                    return true;
                }
            }
            list.Clear();
            return false;
        }
        /*
        public static T GetChildAtPoint2<T>(DependencyObject depObj, Point pt, DependencyObject excludObject = null)
                where T : DependencyObject
        {
            if (depObj == null) return null;

            for (int i = VisualTreeHelper.GetChildrenCount(depObj) - 1; i >= 0; i--)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                if (child != null && child != excludObject)
                {
                    double width = 0;
                    double height = 0;
                    double distance = 6;
                    double top = 0;
                    if (child is Control)
                    {
                        try
                        {
                            width = (child as Control).ActualWidth;
                            height = (child as Control).ActualHeight;
                            if (typeof(T) == typeof(BlockIndicator))
                            {
                                top = (-1) * distance;
                                height += 2 * distance;
                            }
                        }
                        catch (Exception e) { }
                        Point point = ((Control)child).PointFromScreen(pt);
                        if (width > 0 && height > 0 && !(new Rect(0, top, width, height)).Contains(point))
                        {
                            continue;
                        }
                    }
                    {
                        T c = GetChildAtPoint<T>(child as DependencyObject, pt);
                        if (c != null && c is T)
                            return c as T;
                    }
                    if (child is T)
                    {
                        T c = GetChildAtPoint<T>(child as T, pt);
                        if (c != null)
                            return c as T;

                        Point point = ((Visual)child).PointFromScreen(pt);
                        double actualWidth = (child as Control).ActualWidth;
                        double actualHeight = (child as Control).ActualHeight;
                        if(child is ActualSizeAdjustment)
                        {
                            double w2 = 0;
                            double h2 = 0;
                            (child as ActualSizeAdjustment).GetActualSize(out w2, out h2);
                            if (w2 > 0)
                                actualWidth = w2;
                            if (h2 > 0)
                                actualHeight = h2;
                        }
                        Rect rect = new Rect(0, 0, actualWidth, actualHeight);
                        if (typeof(T) == typeof(BlockIndicator))
                        {
                            rect = new Rect(0, -distance, actualWidth,
                                actualHeight + 2 * distance);
                        }
                        if (rect.Contains(point))
                            return child as T;
                    }
                }
            }
            return null;
        }
         */
        // Helper to search up the VisualTree
        public static T FindAnchestor<T>(DependencyObject current, int level = 1)
            where T : DependencyObject
        {
            int index = 0;
            do
            {
                current = VisualTreeHelper.GetParent(current);
                if (current is T)
                {
                    index++;
                    if (index >= level)
                        return current as T;
                }
            }
            while (current != null);
            return null;
        }

        public static FrameworkElement FindChildByName(DependencyObject obj, string name)
        {
            Queue<FrameworkElement> elements = new Queue<FrameworkElement>();
            elements.Enqueue(obj as FrameworkElement);
            int size = 1;
            while (size > 0)
            {
                FrameworkElement e = elements.Dequeue();

                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(e); i++)
                {
                    FrameworkElement child = VisualTreeHelper.GetChild(e, i) as FrameworkElement;
                    if (child.Name == name)
                        return child;
                    if (VisualTreeHelper.GetChildrenCount(child) > 0)
                        elements.Enqueue(child);
                }
                size = elements.Count;
            }
            return null;
        }
        public static Expression CloneExpression(Expression exp)
        {
            return exp.Clone();
        }


        public static Statement CloneStatement(Statement st)
        {
            return st.Clone();
        }
        public static BlockStatement CloneBlockStatement(BlockStatement bs)
        {
            return bs.Clone();
        }

        public static Function CloneFunction(Function st)
        {
            return st.Clone();
        }
        public static Parameter CloneParameter(Parameter pa)
        {
            return pa.Clone();
        }
        
    }
}
