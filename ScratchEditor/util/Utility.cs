using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ScratchNet
{
    class Utility
    {
        
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
            if (exp == null)
                return null;
            Type t = exp.GetType();
            Expression d = Activator.CreateInstance(t) as Expression;
            PropertyInfo[] pinf = t.GetProperties();
            foreach (PropertyInfo p in pinf)
            {
                if (p.CanRead && p.CanWrite)
                {
                    object v = p.GetValue(exp, null);
                    if (v is Expression)
                        v = CloneExpression(v as Expression);
                     p.SetValue(d, v, null);
                }
            }
            return d;
        }


        public static Statement CloneStatement(Statement st)
        {
            Type t = st.GetType();
            Statement d = Activator.CreateInstance(t) as Statement;
            PropertyInfo[] pinf = t.GetProperties();
            foreach (PropertyInfo p in pinf)
            {
                if (p.CanRead && p.CanWrite)
                {
                    object pvalue = p.GetValue(st, null);
                    if (pvalue is Expression)
                    {
                        pvalue = CloneExpression(pvalue as Expression);
                    }
                    else if (pvalue is BlockStatement)
                    {
                        pvalue = CloneBlockStatement(pvalue as BlockStatement);
                    }
                    else if (pvalue is Function)
                    {
                        pvalue = CloneFunction(pvalue as Function);
                    }
                    if (pvalue == null)
                        continue;
                    if (pvalue is List<Expression>)
                    {
                        try
                        {
                            List<Expression> target = pvalue as List<Expression>;
                            List<Expression> nexps = new List<Expression>();
                            foreach (Expression e in target)
                            {
                                nexps.Add(CloneExpression(e));
                            }
                            p.SetValue(d, nexps, null);
                        }
                        catch (Exception ex) { }
                    }
                    else if (pvalue is List<string>)
                    {
                        List<string> target = pvalue as List<string>;
                        List<string> newStr = new List<string>();
                        foreach (string pv in target)
                            newStr.Add(pv);
                        p.SetValue(d, newStr, null);
                    }
                    else
                    {
                        p.SetValue(d, pvalue, null);
                    }

                }
            }
            return d;
        }
        public static BlockStatement CloneBlockStatement(BlockStatement bs)
        {
            Type t = bs.GetType();
            BlockStatement d = Activator.CreateInstance(t) as BlockStatement;
            foreach (Statement cst in bs.Body)
            {
                d.Body.Add(CloneStatement(cst));
            }
            return d;
        }

        public static Function CloneFunction(Function st)
        {
            Type t = st.GetType();
            Function d = Activator.CreateInstance(t) as Function;
            PropertyInfo[] pinf = t.GetProperties();
            foreach (PropertyInfo p in pinf)
            {
                if (p.CanRead && p.CanWrite && p.Name != "Params")
                {
                    object pvalue = p.GetValue(st, null);
                    if (pvalue is Expression)
                    {
                        pvalue = CloneExpression(pvalue as Expression);
                    }
                    else if (pvalue is Statement)
                    {
                        pvalue = CloneStatement(pvalue as Statement);
                    }
                    else if (pvalue is BlockStatement)
                    {
                        pvalue = CloneBlockStatement(pvalue as BlockStatement);
                    }
                    p.SetValue(d, pvalue, null);
                }
            }
            foreach (Parameter p in st.Params)
            {
                d.Params.Add(CloneParameter(p));
            }
            return d;
        }
        public static Parameter CloneParameter(Parameter pa)
        {
            Type t = pa.GetType();
            Parameter d = Activator.CreateInstance(t) as Parameter;
            PropertyInfo[] pinf = t.GetProperties();
            foreach (PropertyInfo p in pinf)
            {
                if (p.CanRead && p.CanWrite)
                {
                    p.SetValue(d, p.GetValue(pa, null), null);
                }
            }
            return d;
        }
        
    }
}
