using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ScratchNet
{
    /// <summary>
    /// Interaction logic for GraphicScriptEditor.xaml
    /// </summary>
    public partial class GraphicScriptEditor : UserControl
    {
        public bool CanEditTypeColor { get; set; } = true;
        public bool IsReadonly { get; set; } = false;
        public GraphicScriptEditor()
        {
            InitializeComponent();
            ScriptContainer.DataContext = this;
            _sprite = null;

            InputManager.Current.PostProcessInput += (sender, e) =>
            {
                /*
                if (e.StagingItem.Input is MouseButtonEventArgs)
                    OnGlobalMouseButtonEventHandler(sender,
                      (MouseButtonEventArgs)e.StagingItem.Input);
                else*/ if (e.StagingItem.Input is MouseEventArgs)
                {
                    OnGlobalMouseMove(sender, (MouseEventArgs)e.StagingItem.Input);
                }

            };

            StepList.MouseLeftButtonDown += (sender, e) =>
            {
                OnGlobalMouseButtonEventHandler(sender, e);
            };
            StepList.MouseLeftButtonUp += (sender, e) =>
            {
                OnGlobalMouseButtonEventHandler(sender, e);
            };
            ScriptContainer.MouseLeftButtonDown += (sender, e) =>
              {
                  OnGlobalMouseButtonEventHandler(sender, e);
              };
            ScriptContainer.MouseLeftButtonUp += (sender, e) =>
            {
                OnGlobalMouseButtonEventHandler(sender, e);
            };
            ScriptBoard.MouseLeftButtonDown += (sender, e) =>
            {
                OnGlobalMouseButtonEventHandler(sender, e);
            };
            ScriptBoard.MouseLeftButtonUp += (sender, e) =>
            {
                OnGlobalMouseButtonEventHandler(sender, e);
            };
            ScriptBoard.MouseRightButtonUp += (sender, e) =>
              {
                  lastPopupMenuPosition.X = e.GetPosition(ScriptBoard).X;
                  lastPopupMenuPosition.Y = e.GetPosition(ScriptBoard).Y;
                  PopupMenu.PlacementRectangle = new Rect(lastPopupMenuPosition.X, lastPopupMenuPosition.Y, 0, 0);
                  CopyCommand.IsEnabled = IsCopyEnabled;
                  PasteCommand.IsEnabled = IsPasteEnabled;
                  PopupMenu.IsOpen = true;
              };
            //generate group, future load from file
            CreateVariableButton = new Button() { Content = "+", Margin = new Thickness(5), Width = 80 };
            CreateVariableButton.Click += CreateVariableButton_Click;
            CreateVariableButton.HorizontalAlignment = HorizontalAlignment.Left;

            CreateFunctionButton = new Button() { Content = "+", Margin = new Thickness(5), Width = 80 };
            CreateFunctionButton.Click += CreateFunctionButton_Click;
            CreateFunctionButton.HorizontalAlignment = HorizontalAlignment.Left;


            functionGroup = null;
            IsCopyEnabled = false;
            IsDeleteEnabled = false;
            IsPasteEnabled = false;
        }
        public static readonly DependencyProperty IsCopyEnabledProperty =
            DependencyProperty.Register("IsCopyEnabled", typeof(Boolean), typeof(GraphicScriptEditor));
        public static readonly DependencyProperty IsDeleteEnabledProperty =
            DependencyProperty.Register("IsDeleteEnabled", typeof(Boolean), typeof(GraphicScriptEditor));
        public static readonly DependencyProperty IsPasteEnabledProperty =
            DependencyProperty.Register("IsPasteEnabled", typeof(Boolean), typeof(GraphicScriptEditor));
        void SelectionChanged()
        {
            if(LastSelectedObject!=null)
            {
                IsCopyEnabled = !(LastSelectedObject is FunctionControl);
                IsDeleteEnabled = true;
            }
            else
            {
                IsCopyEnabled = false;
                IsDeleteEnabled = false;
            }
        }
        public bool IsCopyEnabled
        {
            get
            {
                return (bool)GetValue(IsCopyEnabledProperty);
            }
            set
            {
                SetValue(IsCopyEnabledProperty, value);
            }
        }
        public bool IsPasteEnabled
        {
            get
            {
                return (bool)GetValue(IsPasteEnabledProperty);
            }
            set
            {
                SetValue(IsPasteEnabledProperty, value);
            }
        }
        public bool IsDeleteEnabled
        {
            get
            {
                return (bool)GetValue(IsDeleteEnabledProperty);
            }
            set
            {
                SetValue(IsDeleteEnabledProperty, value);
            }
        }
        public void Print()
        {
            PrintDialog printDlg = new System.Windows.Controls.PrintDialog();

            if (printDlg.ShowDialog() == true)
            {
                //get selected printer capabilities
                System.Printing.PrintCapabilities capabilities = printDlg.PrintQueue.GetPrintCapabilities(printDlg.PrintTicket);
                //get scale of the print wrt to screen of WPF visual
                double scale = Math.Min(capabilities.PageImageableArea.ExtentWidth / ScriptBoard.ActualWidth, capabilities.PageImageableArea.ExtentHeight /
                               ScriptBoard.ActualHeight);
                //Transform the Visual to scale
                //ScriptBoard.LayoutTransform = new ScaleTransform(scale, scale);
                //get the size of the printer page
                Size sz = new Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);
                //update the layout of the visual to the printer page size.
                //ScriptBoard.Measure(sz);
                //Editor.Arrange(new Rect(new Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight), sz));
                //now print the visual to printer to fit on the one page.
                printDlg.PrintVisual(ScriptBoard, "First Fit to Page WPF Print");

            }
        }
        public Node FindParent(Node node)
        {
            return Utility.FindParents(Script, node);
        }
        public Control FindEditorFor(Node node)
        {
            return Utility.FindControl(this.ScriptBoard, node);
        }
        void ClearAllObjectInContainer()
        {
            bool hasObject = false;
            do
            {
                hasObject = false;
                foreach (UIElement c in ScriptContainer.Children)
                {
                    if (c is ExpressionControl || c is StatementControl || c is BlockStatementControl || c is FunctionControl)
                    {
                        ScriptContainer.Children.Remove(c);
                        hasObject = true;
                        break;
                    }
                }
            } while (hasObject);
        }
        List<ScriptStepGroup> toolbar;
        public List<ScriptStepGroup> GetToolBar()
        {
            return toolbar;
        }
        public void SetToolBar(List<ScriptStepGroup> groups)
        {
            toolbar = groups;
            GenerateGroupList(groups);
            //GenerateStepList(groups[0]);
            foreach (ScriptStepGroup g in groups)
            {
                foreach (object b in g.Types)
                {
                    if (b is string)
                    {
                        string a = (string)b;
                        if (a.Equals("CreateFunction"))
                        {
                            CreateFunctionButton.Tag = g;
                            functionGroup = g;
                        }
                        else if (a.Equals("CreateVariable"))
                        {
                            CreateVariableButton.Tag = g;
                        }
                    }
                }
            }
        }
        Class _sprite;
        void ClearBind(DependencyObject chd)
        {
            if (chd is FunctionControl)
            {
                (chd as FunctionControl).Function = null;
            }
            else if (chd is ExpressionControl)
            {
                (chd as ExpressionControl).Expression = null;
            }
            else if (chd is BlockStatementControl)
            {
                (chd as BlockStatementControl).BlockStatement = null;

            }
            ClearAllBinding(chd);
            (chd as Control).DataContext = null;
        }
        private void ClearAllBinding(DependencyObject obj) {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                ClearAllBinding(VisualTreeHelper.GetChild(obj, i));
            }
            BindingOperations.ClearAllBindings(obj);
        }
        public void Highlight(Control control, object value = null)
        {
            Point p = control.TransformToAncestor(ScriptBoard).Transform(new Point(0, 0));
            double x = p.X;// Canvas.GetLeft(control);
            double y = p.Y;// Canvas.GetTop(control);
            double width = control.ActualWidth;
            double height = control.ActualHeight;
            if (control is ActualSizeAdjustment)
            {
                var ac = control as ActualSizeAdjustment;
                double h = 0;
                ac.GetActualSize(out width, out h);
            }
            Highlight(x, y, width, height, (value is object[]) ? null : value);
        }
        public void Highlight(double x, double y, double width, double height, object value = null)
        {
            Canvas.SetLeft(HighlightBorder, x);
            Canvas.SetTop(HighlightBorder, y);
            HighlightBorder.Width = width;
            HighlightBorder.Height = height;
            ValueText.Content = value;
            HighlightBorder.Visibility = Visibility.Visible;
        }
        public void ClearHighlight()
        {
            HighlightBorder.Visibility = Visibility.Collapsed;
        }
        public Class Script
        {
            get
            {
                return _sprite;
            }
            set
            {
                IsCopyEnabled = false;
                IsPasteEnabled = false;
                IsDeleteEnabled = false;
                //ClearBind();
                ScriptBoard.Visibility = Visibility.Collapsed;
                ClearAllObjectInContainer();
                while (ScriptBoard.Children.Count > 1)
                {
                    for (int i = 0; i < ScriptBoard.Children.Count; i++)
                    {
                        UIElement chd = ScriptBoard.Children[i];
                        if (chd.Equals(HighlightBorder))
                            continue;
                        ScriptBoard.Children.RemoveAt(i);
                        ClearBind(chd);
                        chd = null;
                        break;
                    }
                }
                if (CreateFunctionButton.Tag is ScriptStepGroup)
                {
                    (CreateFunctionButton.Tag as ScriptStepGroup).RemoveAllType(typeof(ExpressionStatement));
                    (CreateFunctionButton.Tag as ScriptStepGroup).RemoveAllType(typeof(CallExpression));
                    (CreateFunctionButton.Tag as ScriptStepGroup).RemoveAllType(typeof(ScriptStep));
                }
                if (CreateVariableButton.Tag is ScriptStepGroup)
                {
                    (CreateVariableButton.Tag as ScriptStepGroup).RemoveAllType(typeof(Identifier));
                }
                if (_sprite != null)
                {
                    foreach (Variable v in _sprite.Variables)
                    {
                        CurrentEnviroment.Variables.Remove(v.Name);
                    }
                }
                _sprite = value;
                GC.Collect();
                if (value == null)
                {
                    ScriptBoard.Visibility = Visibility.Visible;
                    return;
                }
                ScriptStepGroup item = CreateVariableButton.Tag as ScriptStepGroup;
                if (item != null)
                {
                    foreach (Variable v in value.Variables)
                    {
                        item.Types.Insert(1, new Identifier() { Variable = v.Name, VarType = v.Type });
                        CurrentEnviroment.Variables.Add(v.Name);
                    }
                }
                /*
                foreach (Resource r in value.Images)
                {
                    CurrentEnviroment.CurrentSpriteImages.Add(r.DisplayName);
                }
                 */
                foreach (Expression e in _sprite.Expressions)
                {
                    Point p;
                    if (_sprite.Positions.ContainsKey(e))
                        p = _sprite.Positions[e];
                    else
                        p = new Point(0, 0);
                    CreateNewExpression(e, p);
                }
                foreach (BlockStatement e in _sprite.BlockStatements)
                {
                    Point p;
                    if (_sprite.Positions.ContainsKey(e))
                        p = _sprite.Positions[e];
                    else
                        p = new Point(0, 0);
                    CreateStatementBlock(e, p);
                }
                foreach (Function e in _sprite.Functions)
                {
                    Point p;
                    if (_sprite.Positions.ContainsKey(e))
                        p = _sprite.Positions[e];
                    else
                        p = new Point(0, 0);
                    CreateNewFunction(e, p);
                    AddFunctionCallStatement(e);
                }
                foreach (EventHandler e in _sprite.Handlers)
                {
                    Point p;
                    if (_sprite.Positions.ContainsKey(e))
                        p = _sprite.Positions[e];
                    else
                        p = new Point(0, 0);
                    CreateNewFunction(e, p);
                }

                if (currentGroup != null)
                {
                    GenerateStepList(currentGroup);
                }

                ScriptBoard.Visibility = Visibility.Visible;
                ScriptBoard.UpdateLayout();
                ChangeCanvasSzie();
                StepListPopup.IsOpen = false;
            }
        }
        void AddFunctionCallStatement(Function func)
        {
            ScriptStepGroup item = CreateFunctionButton.Tag as ScriptStepGroup;
            CallExpression callExpInternal = new CallExpression() { Function = func.Name, FunctionNameFormat = func.Format };
            ExpressionStatement callFunc = new ExpressionStatement() { Expression = callExpInternal };
            CallExpression callExp = new CallExpression() { Function = func.Name, FunctionNameFormat = func.Format };
            foreach (Parameter p in func.Params)
            {
                callExpInternal.ArgTyps.Add(p.Type);
                callExpInternal.Args.Add(new Literal(p.Name));
                callExp.ArgTyps.Add(p.Type);
                callExp.Args.Add(new Literal(p.Name));
            }
            item.Types.Add(callFunc);
            item.Types.Add(new ScriptStep(callExp, true));
        }
        void RemoveFunctionCallStatement(Function func)
        {
            if (functionGroup != null)
            {
                for (int i = 0; i < 3; i++)
                {
                    foreach (object obj in functionGroup.Types)
                    {
                        if (obj is CallExpression)
                        {
                            CallExpression cfs = obj as CallExpression;
                            if (cfs.Function.Equals(func.Name))
                            {
                                functionGroup.Types.Remove(cfs);
                                break;
                            }
                        }
                        else if(obj is ScriptStep)
                        {
                            object exp = (obj as ScriptStep).StepObject;
                            if(exp is CallExpression)
                            {
                                CallExpression cfs = exp as CallExpression;
                                if (cfs.Function.Equals(func.Name))
                                {
                                    functionGroup.Types.Remove(obj);
                                    break;
                                }
                            }
                        }
                        else if (obj is ExpressionStatement)
                        {
                            ExpressionStatement cfs = obj as ExpressionStatement;
                            CallExpression ce = (obj as ExpressionStatement).Expression as CallExpression;
                            if (ce == null)
                                continue;
                            if (ce.Function.Equals(func.Name))
                            {
                                functionGroup.Types.Remove(cfs);
                                break;
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < 3; i++)
            {
                foreach (object obj in StepList.Children)
                {
                    if (obj is StatementControl)
                    {
                        Statement state = (obj as StatementControl).Statement;
                        if (state is ExpressionStatement)
                        {
                            Expression exp = (state as ExpressionStatement).Expression;
                            if (exp is CallExpression)
                            {
                                CallExpression st = exp as CallExpression;
                                if (st != null && st.Function.Equals(func.Name))
                                {
                                    StepList.Children.Remove(obj as Control);
                                    allTypes.Remove(obj);
                                    break;
                                }
                            }
                        }
                    }
                    else if(obj is EditorItemControl)
                    {
                        object exp = (obj as EditorItemControl).EditorObject;
                        if(exp is CallExpression)
                        {
                            CallExpression st = exp as CallExpression;
                            if (st != null && st.Function.Equals(func.Name))
                            {
                                StepList.Children.Remove(obj as Control);
                                allTypes.Remove(obj);
                                break;
                            }
                        }
                    }
                    else if (obj is ExpressionControl)
                    {
                        Expression exp = (obj as ExpressionControl).Expression;
                        if (exp is CallExpression)
                        {
                            CallExpression st = exp as CallExpression;
                            if (st != null && st.Function.Equals(func.Name))
                            {
                                StepList.Children.Remove(obj as Control);
                                allTypes.Remove(obj);
                                break;
                            }
                        }
                    }
                }
            }
        }
        void CreateFunctionButton_Click(object sender, RoutedEventArgs e)
        {
            //StepListPopup.IsOpen = false;
            CreateFunctionDialogEx dlg = new CreateFunctionDialogEx();
            foreach (var f in Script.Functions)
                dlg.ExistFunctions.Add(f.Name);
            if (dlg.ShowDialog() != true)
            {
                return;
            }
            ScriptStepGroup item = CreateFunctionButton.Tag as ScriptStepGroup;
            FunctionDeclaration func = new FunctionDeclaration();
            func.Name = dlg.FunctionName;// Guid.NewGuid()+"";
            string format = "";
            int i = 0;
            foreach (FunctionItem f in dlg.FunctionItems)
            {
                if (f.Type != "text")
                {
                    func.Params.Add(new Parameter() { Name = f.Value, Type = f.Type });
                    format += "[[{{" + i + "}}]]";
                    i++;
                }
                else
                {
                    format += f.Value;
                }
            }
            func.Format = format;
            CreateNewFunction(func, new Point(5, 5));
            /*
            if (func is EventHandler)
            {
                _sprite.Handlers.Add(func as EventHandler);
            }
            else
                _sprite.Functions.Add(func);
                */
            _sprite.Positions.Add(func, new Point(5, 5));
            _sprite.Functions.Add(func);
            AddFunctionCallStatement(func);
            //GenerateStepList(item);
        }

        void CreateVariableButton_Click(object sender, RoutedEventArgs e)
        {
            SetVariableNameDialog dlg = new SetVariableNameDialog();
            dlg.Owner = Application.Current.MainWindow;
            if (dlg.ShowDialog() == true)
            {
                ScriptStepGroup item = CreateVariableButton.Tag as ScriptStepGroup;
                Identifier variable = new Identifier() { Variable = dlg.Variable, VarType = "number|boolean|string" };
                CurrentEnviroment.Variables.Add(variable.Variable);
                item.Types.Insert(1, variable);
                GenerateStepList(item);
                Script.Variables.Add(new VariableDeclarationcs() { Name = dlg.Variable, Type = "number|boolean|string" });
            }
        }
        Button CreateVariableButton;
        Button CreateFunctionButton;
        ScriptStepGroup currentGroup;
        Dictionary<object, object> allTypes = new Dictionary<object, object>();
        void GenerateGroupList(List<ScriptStepGroup> groups)
        {
            GroupList.Children.Clear();
            if (groups == null)
            {
                return;
            }
            foreach (ScriptStepGroup g in groups)
            {
                Button b = new Button();
                b.Content = g.Name;
                b.Tag = g;
                b.MinWidth = 100;
                b.Margin = new Thickness(2, 1, 2, 1);
                b.Padding = new Thickness(10, 5, 10, 5);
                b.Click += GroupButtonClick;
                GroupList.Children.Add(b);
            }
        }

        void GroupButtonClick(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            GenerateStepList((sender as Button).Tag as ScriptStepGroup);
        }
        public event EventHandler<TypeColorChangeEventArgs> TypeColorChanged;
        void EditObjectColor(object obj, Color c)
        {
            if (TypeColorChanged != null)
                TypeColorChanged(this, new TypeColorChangeEventArgs() { Type = obj, Color = c });
        }
        void GenerateStepList(ScriptStepGroup item)
        {
            currentGroup = item;
            List<object> types = item.Types;
            StepList.Children.Clear();
            allTypes.Clear();
            foreach (object obj in types)
            {
                if (obj is Function)
                {
                    FunctionControl func = new FunctionControl() { Function = obj as Function, Margin = new Thickness(5) };
                    StepList.Children.Add(func);
                    allTypes.Add(func, obj);
                }
                else if (obj is Statement)
                {
                    //StatementControl stCrol = new StatementControl() { Statement = obj as Statement, Margin = new Thickness(5) };
                    EditorItemControl stCrol = new EditorItemControl(this);
                    stCrol.IsColorEditable = false;
                    stCrol.EditorObject = obj;
                    StepList.Children.Add(stCrol);
                    allTypes.Add(stCrol, obj);
                }
                else if (obj is Expression)
                {
                    //ExpressionControl expCtrl = new ExpressionControl() { Expression = obj as Expression, Margin = new Thickness(5) };
                    EditorItemControl stCrol = new EditorItemControl(this);
                    stCrol.IsColorEditable = false;
                    stCrol.EditorObject = obj;
                    StepList.Children.Add(stCrol);
                    allTypes.Add(stCrol, obj);
                }
                else if (obj is ScriptStep)
                {
                    ScriptStep step = obj as ScriptStep;
                    EditorItemControl stCrol = new EditorItemControl(this, step.Tooltip);
                    stCrol.IsColorEditable = CanEditTypeColor?step.IsColorEditable:false;
                    stCrol.EditorObject = step.StepObject;
                    stCrol.ColorChanged = EditObjectColor;
                    StepList.Children.Add(stCrol);
                    allTypes.Add(stCrol, step.StepObject);
                }
                else if (obj is UIElement)
                {
                    StepList.Children.Add(obj as UIElement);
                }
                else if (obj is string)
                {
                    if ("CreateVariable".Equals(obj as string))
                    {
                        StepList.Children.Add(CreateVariableButton);
                        CreateVariableButton.Tag = item;
                    }
                    else if ("CreateFunction".Equals(obj as string))
                    {
                        StepList.Children.Add(CreateFunctionButton);
                        //functionGroup = item;
                        CreateFunctionButton.Tag = item;
                    }
                }
            }
            StepListPopup.IsOpen = true;
        }
        //droping
        object lastHoverObject;
        Rect lastHoverObjectRect = new Rect();
        //moving
        Point StartPoint;
        bool IsMoving = false;
        object MovingObject;
        Rect lastClickObjectRegion = new Rect();
        object lastClickedObject;
        Point lastClickedObjectPositio;

        //for removing function and call
        ScriptStepGroup functionGroup;

        private void CheckStatementHover(Point point)
        {
            if (lastHoverObject != null)
            {
                if (Utility.ContainsChilds<BlockIndicator, StatementControl>(lastHoverObject as DependencyObject))
                {

                }
                else if (lastHoverObjectRect.Contains((lastHoverObject as FrameworkElement).PointFromScreen(point)))
                {
                    if (lastHoverObject is StatementControl)
                    {
                        StatementControl child = lastHoverObject as StatementControl; Point pointInChild = child.PointFromScreen(point); // e.GetPosition(child);
                        Rect rect = new Rect(0, 0, (child as Control).ActualWidth,
                            (child as Control).ActualHeight);// 
                        if (pointInChild.Y > (rect.Top + rect.Height / 2) && !child.Statement.IsClosing)
                            child.HoverOnBottom = true;
                        else
                            child.HoverOnTop = true;
                    }
                    else if (lastHoverObject is BlockIndicator)
                    {
                        (lastHoverObject as BlockIndicator).IsHovered = true;
                    }
                    return;
                }
            }
            Rect boardRect = new Rect(0, 0, ScriptBoard.ActualWidth, ScriptBoard.ActualHeight);
            Rect dummy = new Rect();
            lastHoverObject = null;
            if (!boardRect.Contains(ScriptBoard.PointFromScreen(point)))
                return;
            BlockIndicator indicator = Utility.GetChildAtPoint<BlockIndicator>(ScriptBoard, point,
                out dummy,
                MovingObject as DependencyObject);
            if (indicator != null)
            {
                indicator.IsHovered = true;
                lastHoverObject = indicator;
                lastHoverObjectRect = dummy;
            }
            else
            {
                StatementControl child = Utility.GetChildAtPoint<StatementControl>(ScriptBoard, point,
                    out dummy,
                    MovingObject as DependencyObject);
                if (child != null)
                {
                    Point pointInChild = child.PointFromScreen(point); // e.GetPosition(child);
                    Rect rect = new Rect(0, 0, (child as Control).ActualWidth,
                        (child as Control).ActualHeight);// 
                    if (pointInChild.Y > (rect.Top + rect.Height / 2) && !child.Statement.IsClosing)
                        child.HoverOnBottom = true;
                    else
                        child.HoverOnTop = true;
                    lastHoverObject = child;
                    lastHoverObjectRect = dummy;
                }
                else
                {
                }
            }
        }
        private void CheckExpressionHover(Point point, Expression source = null)
        {
            /*
            if (lastHoverObject != null)
            {
                if (lastHoverObjectRect.Contains((lastHoverObject as FrameworkElement).PointFromScreen(point)))
                {
                    if (lastHoverObject is TextBoxExpressionHolder)
                    {
                        (lastHoverObject as TextBoxExpressionHolder).IsHovered = true;
                    }
                    return;
                }
            }
            */
            lastHoverObject = null;
            Rect boardRect = new Rect(0, 0, ScriptBoard.ActualWidth, ScriptBoard.ActualHeight);
            Rect dummy = new Rect();
            if (!boardRect.Contains(ScriptBoard.PointFromScreen(point)))
                return;
            TextBoxExpressionHolder holder = Utility.GetChildAtPoint<TextBoxExpressionHolder>(ScriptBoard, point,
                out dummy,
                MovingObject as DependencyObject);

            if (holder != null)
            {
                string sourceType = source == null ? null : source.ReturnType;
                string targetType = holder.ExpressionDescriptor.Type;
                if(source is VariableDeclarationExpression)
                {
                    if (!holder.CanPlaceVariableDeclaration)
                    {
                        return;
                    }
                    holder.IsDropTypeOK = true;
                }
                else if (CanDrop(targetType, sourceType))
                {
                    holder.IsDropTypeOK = true;
                }
                else
                {
                    holder.IsDropTypeOK = false;
                }
                holder.IsHovered = true;
                lastHoverObject = holder;
            }
        }

        private void ClearIndicator()
        {

            if (lastHoverObject != null)
            {
                if (lastHoverObject is BlockIndicator)
                {
                    (lastHoverObject as BlockIndicator).IsHovered = false;
                }
                if (lastHoverObject is StatementControl)
                {
                    (lastHoverObject as StatementControl).HoverOnBottom = false;
                    (lastHoverObject as StatementControl).HoverOnTop = false;
                }
                if (lastHoverObject is TextBoxExpressionHolder)
                {
                    (lastHoverObject as TextBoxExpressionHolder).IsHovered = false;
                }

            }
        }
        private void ClearArgumentExpression(Statement state, Function parent = null)
        {
            //TO-DO
        }
        //point , on editor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pt">screen point</param>
        /// <returns></returns>
        bool InStepContainer(Point pt)
        {
            Point point = StepContainer.PointFromScreen(pt);
            Rect stepRect = new Rect(0, 0, StepContainer.ActualWidth, StepContainer.ActualHeight);
            return stepRect.Contains(point);
        }
        private bool DropStatement(Statement ex, Point pt, bool createNewControl = true)
        {
            while (true)
            {
                for (int i = 0; i < Script.BlockStatements.Count; i++)
                {
                    if (Script.BlockStatements[i].Body.Count <= 0)
                        Script.BlockStatements.RemoveAt(i);
                    continue;
                }
                break;
            }
            Point point = ScriptBoard.PointToScreen(pt);
            if (InStepContainer(point))
                return false;
            Rect boardRect = new Rect(0, 0, ScriptBoard.ActualWidth, ScriptBoard.ActualHeight);
            if (!boardRect.Contains(pt))
            {
                return false;
            }
            BlockIndicator indicator = lastHoverObject as BlockIndicator;
            StatementControl child;
            if (indicator != null)
            {
                BlockStatement targetBlock = indicator.Tag as BlockStatement;
                targetBlock.Body.Insert(0, ex);
                return true;
            }
            else if ((child = lastHoverObject as StatementControl) != null)
            {
                Rect rect = new Rect(0, 0, child.ActualWidth, child.ActualHeight);// VisualTreeHelper.GetDescendantBounds(child);
                Point pointInChild = child.PointFromScreen(point);
                Statement targetStatement = child.Statement;
                BlockStatementControl bstateCtrl = Utility.FindAnchestor<BlockStatementControl>(child);
                BlockStatement targetBlock = bstateCtrl.BlockStatement;
                if (pointInChild.Y > (rect.Top + rect.Height / 2) && !targetStatement.IsClosing)
                    targetBlock.Body.Insert(targetBlock.Body.IndexOf(targetStatement) + 1, ex);
                else
                    targetBlock.Body.Insert(targetBlock.Body.IndexOf(targetStatement), ex);
                return true;
            }
            else if (createNewControl)
            {
                BlockStatement bstate = new BlockStatement();

                bstate.Body.Add(ex);
                CreateStatementBlock(bstate, pt);
                _sprite.BlockStatements.Add(bstate);
                _sprite.Positions.Add(bstate, pt);
                return true;
            }
            
            return false;
        }
        private bool DropExpression(Expression ex, Point pt, bool createNewControl = true)
        {
            Point point = ScriptBoard.PointToScreen(pt);
            if (InStepContainer(point))
                return false;
            Rect boardRect = new Rect(0, 0, ScriptBoard.ActualWidth, ScriptBoard.ActualHeight);
            Rect dummy = new Rect();
            if (!boardRect.Contains(pt))
            {
                return false;
            }
            TextBoxExpressionHolder holder = lastHoverObject as TextBoxExpressionHolder;
            if (holder != null)
            {
                holder.IsHovered = false;
                string sourceType = ex == null ? null : ex.ReturnType;
                string targetType = holder.ExpressionDescriptor.Type;
                if (ex is VariableDeclarationExpression)
                {
                    if (holder.CanPlaceVariableDeclaration)
                    {
                        Expression old = holder.ExpressionDescriptor.Value as Expression;
                        holder.ExpressionDescriptor.Value = ex;
                        return true;
                    }
                    return false;
                }
                else{
                    if (CanDrop(targetType, sourceType))
                    {
                        Expression old = holder.ExpressionDescriptor.Value as Expression;
                        holder.ExpressionDescriptor.Value = ex;
                        if (old != null)
                        {
                            //CreateNewExpression(old, new Point(pt.X + 20, pt.Y + 20));
                        }
                        return true;
                    }
                }
            }
            //if (ex is ArgumentExpression)
            //{
            //    return false;
            //}
            if (createNewControl)
            {
                CreateNewExpression(ex, pt);
                _sprite.Expressions.Add(ex);
                _sprite.Positions.Add(ex, pt);
                return true;
            }
            return false;
        }
        private void CreateNewExpression(Expression ex, Point pt)
        {
            ExpressionControl ectrl = new ExpressionControl();
            ectrl.Expression = ex;
            ScriptBoard.Children.Add(ectrl);
            //ectrl.Margin = new Thickness(pt.X, pt.Y, 0, 0);
            Canvas.SetLeft(ectrl, pt.X);
            Canvas.SetTop(ectrl, pt.Y);
        }
        private void CreateStatementBlock(BlockStatement bstate, Point pt)
        {
            BlockStatementControl bstCtrl = new BlockStatementControl();
            bstCtrl.BlockStatement = bstate;
            ScriptBoard.Children.Add(bstCtrl);
            Canvas.SetLeft(bstCtrl, pt.X);
            Canvas.SetTop(bstCtrl, pt.Y);
        }
        private void CreateNewFunction(Function fc, Point pt)
        {
            FunctionControl ectrl = new FunctionControl();
            ectrl.Function = fc;
            Canvas.SetLeft(ectrl, pt.X);
            Canvas.SetTop(ectrl, pt.Y);

            ScriptBoard.Children.Add(ectrl);
        }
        private bool DropFunction(Function fc, Point pt)
        {
            Point point = ScriptBoard.PointToScreen(pt);
            Rect boardRect = new Rect(0, 0, ScriptBoard.ActualWidth, ScriptBoard.ActualHeight);
            if (!boardRect.Contains(pt) || InStepContainer(point))
            {
                if (fc is EventHandler)
                    return false;
                else if (CallCommandExist((fc as Function).Name))
                {
                    MessageBox.Show("The function is used and can not be deleted\nPlease delete the function call", "Function can't be deleted", MessageBoxButton.OK, MessageBoxImage.Warning);

                    pt = lastClickedObjectPositio;
                }
                else
                {
                    return false;
                }
            }
            CreateNewFunction(fc, pt);
            if (fc is EventHandler)
            {
                _sprite.Handlers.Add(fc as EventHandler);
            }
            else
                _sprite.Functions.Add(fc);
            _sprite.Positions.Add(fc, pt);
            return true;
        }
        bool CallCommandExist(string func)
        {
            foreach (var s in Script.BlockStatements)
            {
                if (CallCommandInObject(func, s))
                    return true;
            }
            foreach (var s in Script.Expressions)
            {
                if (CallCommandInObject(func, s))
                    return true;
            }
            foreach (var s in Script.Functions)
            {
                if (CallCommandInObject(func, s))
                    return true;
            }
            return false;
        }
        bool CallCommandInObject(string func, object script)
        {
            if (script == null)
                return false;
            if ((script is CallExpression) && (script as CallExpression).Function == func)
                return true;
            PropertyInfo[] pinfo = script.GetType().GetProperties();
            foreach (PropertyInfo p in pinfo)
            {
                if (p.PropertyType == typeof(Statement))
                {
                    Statement st = p.GetValue(script, null) as Statement;
                    if (CallCommandInObject(func, st))
                        return true;
                }
                else if (p.PropertyType == typeof(Expression))
                {
                    Expression st = p.GetValue(script, null) as Expression;
                    if (CallCommandInObject(func, st))
                        return true;
                }
                else if (p.PropertyType == typeof(BlockStatement))
                {
                    BlockStatement bs = p.GetValue(script, null) as BlockStatement;
                    if (bs != null)
                    {
                        foreach (Statement st in bs.Body)
                        {
                            if (CallCommandInObject(func, st))
                                return true;
                        }
                    }
                }
            }
            return false;
        }
        private bool CanDrop(string targetType, string sourceType)
        {
            return true;
            if (targetType == null || sourceType == null)
                return true;
            string[] targets = targetType.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            string[] sources = sourceType.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string t in targets)
                foreach (string s in sources)
                    if (s.Equals(t, StringComparison.OrdinalIgnoreCase))
                        return true;
            return false;
        }

        object PickTargetInScriptBoard(Point pt, out Rect region)
        {
            Rect boardRect = new Rect(0, 0, ScriptBoard.ActualWidth, ScriptBoard.ActualHeight);
            region = new Rect();
            if (!boardRect.Contains(pt))
            {
                return null;
            }
            Point point = ScriptBoard.PointToScreen(pt);
            ParameterIndicator paramIndicator = Utility.GetChildAtPoint<ParameterIndicator>(ScriptBoard, point, out region);
            if (paramIndicator != null)
                return paramIndicator;
            TextBoxExpressionHolder holder = Utility.GetChildAtPoint<TextBoxExpressionHolder>(ScriptBoard, point,
                out region, null);
            if (holder != null && holder.ExpressionDescriptor.Value != null)
            {
                return holder;
            }
            else if (holder != null)
            {
                //TextBoxExpressionHolder hoder2 = FindAnchestor<TextBoxExpressionHolder>(ScriptBoard);
                //if (hoder2 != null && hoder2.ExpressionDescriptor.Value != null)
                //    return hoder2;
            }
            ExpressionControl expCtrl = Utility.GetChildAtPoint<ExpressionControl>(ScriptBoard, point, out region);
            if (expCtrl != null)// && ScriptBoard.Children.Contains(expCtrl))
            {
                return expCtrl;
            }
            StatementControl child = Utility.GetChildAtPoint<StatementControl>(ScriptBoard, point, out region);
            if (child != null)
            {
                return child;
            }

            FunctionControl funcCtrol = Utility.GetChildAtPoint<FunctionControl>(ScriptBoard, point, out region);
            if (funcCtrol != null)
            {
                return funcCtrol;
            }
            return null;
        }
        object PickTargetInStepList(Point point)
        {
            foreach (object ctrl in allTypes.Keys)
            {
                Rect ctrlRect = new Rect(0, 0, (ctrl as Control).ActualWidth, (ctrl as Control).ActualHeight);
                if (ctrlRect.Contains((ctrl as Control).PointFromScreen(point)))
                {
                    return ctrl;
                }
            }
            return null;
        }
        private void OnGlobalMouseButtonEventHandler(object sender, MouseButtonEventArgs e)
        {
            if (!this.IsVisible)
                return;
            if (e.Handled)
                return;
            if (IsReadonly)
                return;
            lastClickedObject = null;
            ClearIndicator();
            Point pt = e.GetPosition(ScriptBoard);
            Point point = ScriptBoard.PointToScreen(pt);
            /*
            if(e.LeftButton==MouseButtonState.Pressed && IsMoving)
            {
                if (MovingObject != null)
                {
                    ClearAllObjectInContainer();
                    MovingObject = null;
                    DeleteIndicator.Visibility = Visibility.Collapsed;
                    //change canvas size
                    UpdateLayout();
                    ChangeCanvasSzie();
                    lastClickedObject = null;
                    IsMoving = false;
                }
            }*/
            e.Handled = true;
            if (e.LeftButton == MouseButtonState.Pressed && !IsMoving)
            {
                StartPoint = e.GetPosition(null);
                IsMoving = false;
                if (LastSelectedObject !=null)
                {
                    LastSelectedObject = null;
                }
                //避免Combox鼠标操作，被干扰
                if (StepListPopup.IsOpen)
                {
                    try
                    {
                        Rect stepRect = new Rect(0, 0, StepList.ActualWidth, StepList.ActualHeight);
                        Point ptInList = e.GetPosition(StepList);
                        if (stepRect.Contains(ptInList))
                        {
                            lastClickedObject = PickTargetInStepList(point);
                            e.Handled = true;
                            return;
                        }
                        else
                        {
                            StepListPopup.IsOpen = false;
                            e.Handled = true;
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        MessageBox.Show(ex.StackTrace);
                    }
                    e.Handled = true;
                    return;
                }
                Rect pbRect = new Rect(0, 0, ScriptBoard.ActualWidth, ScriptBoard.ActualHeight);
                if (!pbRect.Contains(pt))
                {
                    return;
                }
                lastClickedObject = PickTargetInScriptBoard(pt, out lastClickObjectRegion);
                if (lastClickedObject != null)
                {
                    if (ScriptBoard.Children.Contains(lastClickedObject as Control))
                    {
                        lastClickedObjectPositio = new Point(Canvas.GetLeft(lastClickedObject as Control),
                            Canvas.GetTop(lastClickedObject as Control));
                    }
                    if (lastClickedObject is ISelectable)
                    {
                        LastSelectedObject = lastClickedObject;
                    }
                }
            }
            else if ((e.LeftButton == MouseButtonState.Pressed && IsMoving) || (e.LeftButton == MouseButtonState.Released && IsMoving))
            {
                IsMoving = false;
                LastSelectedObject = null;
                if (MovingObject is ExpressionControl)
                {
                    Expression exp = Utility.CloneExpression((MovingObject as ExpressionControl).Expression);
                    bool added=DropExpression(exp, pt, true);
                    ScriptContainer.Children.Remove(MovingObject as ExpressionControl);

                    e.Handled = true;
                    if (added)
                        SetNameForVariableDeclaration(exp);
                    IsModified = true;
                }
                else if (MovingObject is BlockStatementControl)
                {
                    BlockStatementControl bkCtrl = MovingObject as BlockStatementControl;
                    Statement state = Utility.CloneStatement(bkCtrl.BlockStatement.Body[0]);
                    bool added = DropStatement(state, pt, true);
                    ScriptContainer.Children.Remove(bkCtrl);
                    e.Handled = true;
                    if (added)
                        SetNameForVariableDeclaration(state);
                    IsModified = true;
                }
                else if (MovingObject is Image)
                {
                    Image bkCtrl = MovingObject as Image;
                    Statement state = Utility.CloneStatement(bkCtrl.Tag as Statement);
                    bool added = DropStatement(state, pt, true);
                    ScriptContainer.Children.Remove(bkCtrl);
                    e.Handled = true;
                    if (added)
                        SetNameForVariableDeclaration(state);
                    IsModified = true;
                }
                else if (MovingObject is FunctionControl)
                {
                    FunctionControl funcCtrl = MovingObject as FunctionControl;
                    Function func = Utility.CloneFunction(funcCtrl.Function);
                    if (!DropFunction(func, pt))
                    {
                        RemoveFunctionCallStatement(func);
                    }
                    ScriptContainer.Children.Remove(funcCtrl);
                    e.Handled = true;
                    IsModified = true;
                }
                MovingObject = null;
                DeleteIndicator.Visibility = Visibility.Collapsed;
                //change canvas size
                UpdateLayout();
                ChangeCanvasSzie();
                lastClickedObject = null;
                IsMoving = false;
            }
            else
            {
                lastClickedObject = null;
                IsMoving = false;
            }
        }
        object _lastSelectedObject = null;
        object LastSelectedObject
        {
            get
            {
                return _lastSelectedObject;
            }
            set
            {
                if(_lastSelectedObject!=null && _lastSelectedObject is ISelectable)
                {
                    (_lastSelectedObject as ISelectable).IsSelected = false;
                }
                _lastSelectedObject = value;
                if (_lastSelectedObject != null && _lastSelectedObject is ISelectable)
                {
                    (_lastSelectedObject as ISelectable).IsSelected = true;
                }
                SelectionChanged();
            }
        }
        void SetNameForVariableDeclaration(Statement state)
        {
            /*
            if (state is VariableDeclarationStatment)
            {
                var c = state as VariableDeclarationStatment;
                var v = c.Variable;
                if (v == null || (v is Identifier && string.IsNullOrEmpty((v as Identifier).Variable)))
                {
                    SetVariableNameDialog dlg = new SetVariableNameDialog();
                    if (dlg.ShowDialog() == true)
                    {
                        c.Variable = new Identifier() { Variable = dlg.Variable };
                    }
                    else
                        c.Variable = new Identifier() { Variable = "variable" };
                }
                //c.FireNameChange();
            }*/
            if(state is ExpressionStatement)
            {
                Expression exp = (state as ExpressionStatement).Expression;
                if(exp is VariableDeclarationExpression)
                {
                    SetNameForVariableDeclaration(exp);
                }
            }
        }
        void SetNameForVariableDeclaration(Expression exp)
        {
            if (exp is VariableDeclarationExpression)
            {
                var c = exp as VariableDeclarationExpression;
                var v = c.Variable;
                if (v == null || (v is Identifier && string.IsNullOrEmpty((v as Identifier).Variable)))
                {
                    SetVariableNameDialog dlg = new SetVariableNameDialog();
                    if (dlg.ShowDialog() == true)
                    {
                        c.Variable = new Identifier() { Variable = dlg.Variable };
                    }
                    else
                        c.Variable = new Identifier() { Variable = "variable" };
                }
                //c.FireNameChange();
            }
        }
        void ChangeCanvasSzie()
        {
            double width = ScriptScroller.ActualWidth; //ScriptBoard.ActualWidth;
            double height = ScriptScroller.ActualHeight;// ScriptBoard.ActualHeight;
            foreach (UIElement ctrl in ScriptBoard.Children)
            {
                if (ctrl is Control)
                {
                    Rect rect = VisualTreeHelper.GetDescendantBounds(ctrl);
                    double w = Canvas.GetLeft(ctrl) + rect.Width + 400;
                    double h = Canvas.GetTop(ctrl) + rect.Height + 400;
                    if (w > width) width = w;
                    if (h > height) height = h;
                }
            }

            ScriptBoard.Width = width;
            ScriptBoard.Height = height;
        }
        void StartMoveExpression(Expression exp, Point ptInCnt)
        {
            ExpressionControl newControl = new ExpressionControl();
            ScriptContainer.Children.Add(newControl);
            newControl.Expression = exp;
            newControl.LayoutTransform = ScriptBoard.LayoutTransform;
            Canvas.SetLeft(newControl, ptInCnt.X);
            Canvas.SetTop(newControl, ptInCnt.Y);
            Canvas.SetZIndex(newControl, 99);
            MovingObject = newControl;
            IsMoving = true;
            DeleteIndicator.Visibility = Visibility.Visible;
            if (_sprite.Expressions.Contains(exp))
            {
                _sprite.Expressions.Remove(exp);
                _sprite.Positions.Remove(exp);
            }
        }
        void StartMoveStatement(Statement state, Point ptInCnt, bool createNew=false)
        {
            if (createNew)
            {
                BlockStatement block = new BlockStatement();
                block.Body.Add(state);
                BlockStatementControl bkCtrl = new BlockStatementControl()
                {
                    BlockStatement = block
                };
                bkCtrl.LayoutTransform = ScriptBoard.LayoutTransform;
                ScriptContainer.Children.Add(bkCtrl);
                Canvas.SetLeft(bkCtrl, ptInCnt.X);
                Canvas.SetTop(bkCtrl, ptInCnt.Y);
                Canvas.SetZIndex(bkCtrl, 99);
                MovingObject = bkCtrl;
                IsMoving = true;
                DeleteIndicator.Visibility = Visibility.Visible;
                BlockStatement bs = null;
                foreach (BlockStatement v in _sprite.BlockStatements)
                    if (v.Body.Contains(state))
                        bs = v;
                if (bs != null && bs.Body.Count == 1)
                {
                    _sprite.BlockStatements.Remove(bs);
                    _sprite.Positions.Remove(bs);
                }
            }
            else
            {
                bool complex = false;
                StatementControl child = lastClickedObject as StatementControl;
                Type type = state.GetType();
                foreach(var p in type.GetProperties())
                {
                    if(p.PropertyType.IsAssignableFrom(typeof(BlockStatement)))
                    {
                        complex = true;
                        break;
                    }
                }
                if (complex)
                {
                    Image image = new Image();
                    image.Tag = state;
                    image.Source = DrawVisualUtil.VisualToImageSource(child);
                    BlockStatementControl pCtrol = Utility.FindAnchestor<BlockStatementControl>(child);
                    if (pCtrol.BlockStatement.Body.Count == 0 && ScriptBoard.Children.Contains(pCtrol))
                    {
                        ScriptBoard.Children.Remove(pCtrol);
                    }
                    else
                    {
                        pCtrol.BlockStatement.Body.Remove(child.Statement);
                    }

                    image.LayoutTransform = ScriptBoard.LayoutTransform;
                    ScriptContainer.Children.Add(image);
                    Canvas.SetLeft(image, ptInCnt.X);
                    Canvas.SetTop(image, ptInCnt.Y);
                    Canvas.SetZIndex(image, 99);
                    MovingObject = image;
                }
                else
                {
                    BlockStatementControl pCtrol = Utility.FindAnchestor<BlockStatementControl>(child);
                    if (pCtrol.BlockStatement.Body.Count == 0 && ScriptBoard.Children.Contains(pCtrol))
                    {
                        ScriptBoard.Children.Remove(pCtrol);
                    }
                    else
                    {
                        pCtrol.BlockStatement.Body.Remove(child.Statement);
                    }

                    BlockStatement block = new BlockStatement();
                    block.Body.Add(state);
                    BlockStatementControl bkCtrl = new BlockStatementControl()
                    {
                        BlockStatement = block
                    };
                    bkCtrl.LayoutTransform = ScriptBoard.LayoutTransform;
                    ScriptContainer.Children.Add(bkCtrl);
                    Canvas.SetLeft(bkCtrl, ptInCnt.X);
                    Canvas.SetTop(bkCtrl, ptInCnt.Y);
                    Canvas.SetZIndex(bkCtrl, 99);
                    MovingObject = bkCtrl;
                }
                IsMoving = true;
                DeleteIndicator.Visibility = Visibility.Visible;
                BlockStatement bs = null;
                foreach (BlockStatement v in _sprite.BlockStatements)
                    if (v.Body.Contains(state))
                        bs = v;
                if (bs != null && bs.Body.Count == 1)
                {
                    _sprite.BlockStatements.Remove(bs);
                    _sprite.Positions.Remove(bs);
                }
            }
        }
        void StartMoveFunction(Function func, Point ptInCnt, bool createNew=false)
        {
            
            if (func is EventHandler)
            {
                _sprite.Handlers.Remove(func as EventHandler);
                _sprite.Positions.Remove(func);
            }
            else
            {
                _sprite.Functions.Remove(func);
                _sprite.Positions.Remove(func);
            }
            FunctionControl funcCtrl;
            if (!createNew)
            {
                funcCtrl = FindEditorFor(func) as FunctionControl;
                ScriptBoard.Children.Remove(funcCtrl);
            }
            else
                funcCtrl = new FunctionControl() { Function = func };
            funcCtrl.LayoutTransform = ScriptBoard.LayoutTransform;
            ScriptContainer.Children.Add(funcCtrl);
            
            Canvas.SetLeft(funcCtrl, ptInCnt.X);
            Canvas.SetTop(funcCtrl, ptInCnt.Y);
            Canvas.SetZIndex(funcCtrl, 99);
            MovingObject = funcCtrl;
            IsMoving = true;
            DeleteIndicator.Visibility = Visibility.Visible;
        }
        private void OnGlobalMouseMove(object sender, MouseEventArgs e)
        {
            if (IsReadonly)
                return;
            if (e.Handled)
                return;
            if (!this.IsVisible)
                return;
            Point mousePos = e.GetPosition(null);
            if (mousePos.X < 0 || mousePos.Y < 0 || StartPoint.X < 0 || StartPoint.Y < 0)
            {
                return;
            }
            Vector diff = StartPoint - mousePos;
            Point pt = e.GetPosition(ScriptBoard);
            if (lastClickedObject == null)
                return;
            if (IsMoving)
            {
                //仪出编辑范围，就没法捕获鼠标事件，只能删除
                var pts = e.GetPosition(ScriptContainer);
                var pps = e.GetPosition(StepListPopup);
                if (!ScriptContainer.IsMouseCaptured)
                {

                }
                else if ((pts.X < 0 || pts.Y < 0 || pts.X > ScriptContainer.ActualWidth || pts.Y > ScriptContainer.ActualHeight))
                {
                    if(MovingObject!=null)
                        ScriptContainer.Children.Remove(MovingObject as Control);
                    MovingObject = null;
                    DeleteIndicator.Visibility = Visibility.Collapsed;
                    lastClickedObject = null;
                    IsMoving = false;
                    ClearIndicator();
                    IsModified = true;
                    return;
                }
                ClearIndicator();
                Point ptInCnt = e.GetPosition(ScriptContainer);
                UIElement newControl = MovingObject as UIElement;

                Point point = ScriptBoard.PointToScreen(pt);
                //Console.WriteLine(point);
                if (MovingObject is ExpressionControl)
                {
                    CheckExpressionHover(point, (MovingObject as ExpressionControl).Expression);
                    Canvas.SetLeft(newControl, ptInCnt.X);
                    Canvas.SetTop(newControl, ptInCnt.Y);
                }
                else if (MovingObject is BlockStatementControl)
                {
                    CheckStatementHover(point);
                    Canvas.SetLeft(newControl, ptInCnt.X);
                    Canvas.SetTop(newControl, ptInCnt.Y);
                }
                else if (MovingObject is EventHandler)
                {
                    Canvas.SetLeft(newControl, ptInCnt.X);
                    Canvas.SetTop(newControl, ptInCnt.Y);
                }
                else if (MovingObject is FunctionControl)
                {
                    FunctionControl funCtr = MovingObject as FunctionControl;
                    Canvas.SetLeft(newControl, ptInCnt.X);
                    Canvas.SetTop(newControl, ptInCnt.Y);
                }
                else if(MovingObject is Image)
                {
                    CheckStatementHover(point);
                    Canvas.SetLeft(newControl, ptInCnt.X);
                    Canvas.SetTop(newControl, ptInCnt.Y);
                }
                return;
            }
            if (e.LeftButton == MouseButtonState.Pressed && lastClickedObject != null &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                Point point = ScriptBoard.PointToScreen(pt);
                Point ptInCnt = ScriptContainer.PointFromScreen(point);
                lastHoverObject = null;
                if (StepListPopup.IsOpen)
                {
                    try
                    {
                        Rect stepRect = new Rect(0, 0, StepList.ActualWidth, StepList.ActualHeight);
                        Point ptInList = e.GetPosition(StepList);
                        if (stepRect.Contains(ptInList))
                        {
                            object dragType = lastClickedObject;// PickTargetInStepList(point);
                            if (dragType == null)
                                return;
                            ClearAllObjectInContainer();
                            Object obj;
                            if (allTypes.ContainsKey(dragType))
                            {
                                obj = allTypes[dragType];
                            }
                            else
                                return;
                            if (obj is Expression)
                            {
                                Expression exp = Utility.CloneExpression(obj as Expression);
                                StartMoveExpression(exp, ptInCnt);
                                e.Handled = true;
                                StepListPopup.IsOpen = false;
                                return;
                            }
                            else if (obj is Statement)
                            {
                                Statement state = Utility.CloneStatement(obj as Statement);
                                StartMoveStatement(state, ptInCnt, true);
                                e.Handled = true;
                                StepListPopup.IsOpen = false;
                                return;
                            }
                            else if (obj is Function)
                            {
                                Function handler = Utility.CloneFunction(obj as Function);
                                StartMoveFunction(handler, ptInCnt, true);
                                e.Handled = true;
                                StepListPopup.IsOpen = false;
                                return;
                            }
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                    }
                }
                Rect pbRect = new Rect(0, 0, ScriptBoard.ActualWidth, ScriptBoard.ActualHeight);
                if (!pbRect.Contains(pt))
                {
                    return;
                }
                object dragObject = null;
                if (lastClickedObject != null)
                {
                    dragObject = lastClickedObject;
                }
                else
                    return;
                if (dragObject is TextBoxExpressionHolder)
                {
                    TextBoxExpressionHolder holder = dragObject as TextBoxExpressionHolder;
                    Expression exp = holder.ExpressionDescriptor.Value as Expression;
                    if (exp == null)
                        return;
                    holder.ExpressionDescriptor.Value = null;

                    StartMoveExpression(exp, ptInCnt);
                    //e.Handled = true;
                    return;
                }
                else if (dragObject is ExpressionControl)
                {
                    ExpressionControl expCtrl = dragObject as ExpressionControl;
                    if (ScriptBoard.Children.Contains(expCtrl))
                    {
                        Expression exp = expCtrl.Expression;
                        ScriptBoard.Children.Remove(expCtrl);
                        if (exp == null)
                            return;
                        StartMoveExpression(exp, ptInCnt);
                        //e.Handled = true;
                        return;
                    }
                    else
                    {
                        TextBoxExpressionHolder holder = Utility.FindAnchestor<TextBoxExpressionHolder>(expCtrl);
                        if (holder != null)
                        {
                            Expression exp = holder.ExpressionDescriptor.Value as Expression;
                            holder.ExpressionDescriptor.Value = null;
                            if (exp == null)
                                return;
                            StartMoveExpression(exp, ptInCnt);
                            //e.Handled = true;
                            return;
                        }
                    }
                }
                else if (dragObject is ParameterIndicator)
                {
                    ParameterIndicator pInd = dragObject as ParameterIndicator;
                    ParameterDescriptor pd = pInd.Parameter;
                    Identifier exp = new Identifier();
                    exp.VarType = pd.Type;
                    exp.Variable = pd.Name;
                    if (exp == null)
                        return;
                    StartMoveExpression(exp, ptInCnt);
                    //e.Handled = true;
                    return;
                }
                else if (dragObject is StatementControl)
                {
                    StatementControl child = dragObject as StatementControl;
                    if (lastClickedObject == child)
                    {
                        Statement state = child.Statement;
                        if (state == null)
                            return;
                        
                        StartMoveStatement(state, ptInCnt);

                        //e.Handled = true;
                    }
                }
                else if (dragObject is FunctionControl)
                {
                    FunctionControl funcControl = dragObject as FunctionControl;
                    if (funcControl != null)
                    {
                        StartMoveFunction(funcControl.Function, ptInCnt);
                        ScriptBoard.Children.Remove(funcControl);
                        IsMoving = true;
                        //e.Handled = true;
                    }
                }
            }

        }

        private void OnZoomIn(object sender, RoutedEventArgs e)
        {
            if (ScriptBoard.LayoutTransform is ScaleTransform)
            {
                ScaleTransform scale = ScriptBoard.LayoutTransform as ScaleTransform;
                double f = scale.ScaleX - 0.1;
                if (f < 0.2)
                    f = 0.2;
                if (ScriptBoard.Width * f < ScriptScroller.ActualWidth ||
                    ScriptBoard.Height * f < ScriptScroller.ActualHeight)
                    return;
                scale.ScaleX = f;
                scale.ScaleY = f;
            }
        }

        private void OnZoomOut(object sender, RoutedEventArgs e)
        {
            if (ScriptBoard.LayoutTransform is ScaleTransform)
            {
                ScaleTransform scale = ScriptBoard.LayoutTransform as ScaleTransform;
                double f = scale.ScaleX + 0.1;
                scale.ScaleX = f;
                scale.ScaleY = f;
            }
        }
        private void OnZoomNon(object sender, RoutedEventArgs e)
        {
            if (ScriptBoard.LayoutTransform is ScaleTransform)
            {
                ScaleTransform scale = ScriptBoard.LayoutTransform as ScaleTransform;
                double f = 1;
                scale.ScaleX = f;
                scale.ScaleY = f;
            }
        }
        /// <summary>
        /// script editor scale factor
        /// </summary>
        public double ScacleRatio { get
            {
                ScaleTransform scale = ScriptBoard.LayoutTransform as ScaleTransform;
                return scale.ScaleX;
            }
            set
            {
                ScaleTransform scale = ScriptBoard.LayoutTransform as ScaleTransform;
                scale.ScaleY = value;
                scale.ScaleX = value;
            }
        }
        public bool IsModified { get; set; } = false;
        public bool IsLibraryEnabled
        {
            get
            {
                return LibraryButton.Visibility == Visibility.Visible;
            }
            set
            {
                LibraryButton.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        private void OnLibrary(object sender, RoutedEventArgs e)
        {
            ChangeImportLibrary?.Invoke(sender, e);
        }
        public event EventHandler<EventArgs> ChangeImportLibrary;
        public void Copy()
        {
            if (_lastSelectedObject != null)
            {
                var obj = (_lastSelectedObject as ISelectable).SelectedValue;
                lastCopyObject = null;
                if (obj is Expression)
                {
                    lastCopyObject = (obj as Expression).Clone();
                }
                else if (obj is Statement)
                    lastCopyObject = (obj as Statement).Clone();
                else if (obj is Function)
                    lastCopyObject = (obj as Function).Clone();
                IsPasteEnabled = lastCopyObject != null;
            }
        }
        public void Delete()
        {

        }
        public void Paste(Point position)
        {
            if(lastCopyObject is Statement)
            {
                Statement ex = (lastCopyObject as Statement).Clone();
                BlockStatement bstate = new BlockStatement();
                bstate.Body.Add(ex);
                CreateStatementBlock(bstate, position);
                _sprite.BlockStatements.Add(bstate);
                _sprite.Positions.Add(bstate, position);
            }
            else if(lastCopyObject is Expression)
            {
                Expression ex = (lastCopyObject as Expression).Clone();
                CreateNewExpression(ex, position);
                _sprite.Expressions.Add(ex);
                _sprite.Positions.Add(ex, position);
            }
        }
        private void OnCopy(object sender, RoutedEventArgs e)
        {
            Copy();
        }

        private void OnPaste(object sender, RoutedEventArgs e)
        {
            Paste(lastPopupMenuPosition);
        }

        private void OnDelete(object sender, RoutedEventArgs e)
        {
            Delete();
        }
        Point lastPopupMenuPosition;
        Node lastCopyObject;
    }
}
