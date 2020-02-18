using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ScratchNet
{
    class ScriptControlDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate StatementTemplate { get; set; }
        public DataTemplate ExpressionTemplate { get; set; }
        public DataTemplate FunctionTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
                return base.SelectTemplate(item, container);
            if (item is Statement)
                return StatementTemplate;
            if (item is Expression)
                return ExpressionTemplate;
            if (item is Function)
                return FunctionTemplate;
            return base.SelectTemplate(item, container);
        }
    }
}
