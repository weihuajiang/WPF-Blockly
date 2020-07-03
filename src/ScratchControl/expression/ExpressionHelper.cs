using ScratchNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ScratchControl
{
    public class ExpressionHelper
    {
        public static ExpressionControl Build(ScratchNet.Expression expression)
        {
            if ("boolean".Equals(expression.Type))
            {
                return new BooleanExpressionControl() { Expression = expression };
            }
            if ("number".Equals(expression.Type))
                return new NumberExpressionControl() { Expression = expression };
            return new NumberExpressionControl() { Expression = expression };
        }
        internal static void SetupExpressionControl(ScratchNet.Expression expresion, StackPanel container)
        {
            foreach (ItemDescriptor d in expresion.Descriptor)
            {
                container.Children.Add(GetUIFor(d));
            }
        }
        internal static UIElement GetUIFor(ItemDescriptor item)
        {
            if (item is TextItemDescriptor)
            {
                TextBlock b = new TextBlock();
                b.Text = (item as TextItemDescriptor).Text;
                b.VerticalAlignment = VerticalAlignment.Center;
                return b;
            }
            if (item is ExpressionDescriptor)
            {
                ExpressionDescriptor d = item as ExpressionDescriptor;
                if (d.Type == "boolean")
                {
                    BooleanExpressionHolder holder = new BooleanExpressionHolder();
                    holder.Descriptor = (item as ExpressionDescriptor);
                    return holder;
                }
                if (d.Type == "number")
                {
                    NumberExpressionHolder holder = new NumberExpressionHolder();
                    holder.Descriptor = (item as ExpressionDescriptor);
                    return holder;
                }
                ObjectExpressionHolder h = new ObjectExpressionHolder();
                h.Descriptor = item as ExpressionDescriptor;
                return h;
            }
            if (item is SelectionItemDescriptor)
            {
                SelectionItemDescriptor selItem = item as SelectionItemDescriptor;
                return new SelectionItemControl() { Descriptor = selItem };
            }
            if (item is ImageItemDescriptor)
            {
                Image img = new Image();
                img.Height = 16;
                img.Source = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + System.IO.Path.DirectorySeparatorChar + (item as ImageItemDescriptor).Path));
            }
            if(item is VariableDescriptor){

            }
            if (item is ParameterDescriptor)
            {

            }
            return null;
        }
    }
}
