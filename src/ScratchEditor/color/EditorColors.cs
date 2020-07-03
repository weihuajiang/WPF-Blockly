using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ScratchNet
{
    public static class EditorColorsExtension
    {
        public static void Register<A>(this GraphicScriptEditor editor, Color b)
        {
            EditorColors.Register<A>(b);
        }
        public static void Register<A,B>(this GraphicScriptEditor editor, Color b)
        {
            EditorColors.Register<A>(b);
            EditorColors.Register<B>(b);
        }
        public static void Register<A, B, C>(this GraphicScriptEditor editor, Color b)
        {
            EditorColors.Register<A>(b);
            EditorColors.Register<B>(b);
            EditorColors.Register<C>(b);
        }
        public static void Register<A, B, C,D>(this GraphicScriptEditor editor, Color b)
        {
            EditorColors.Register<A>(b);
            EditorColors.Register<B>(b);
            EditorColors.Register<C>(b);
            EditorColors.Register<D>(b);
        }
        public static void Register<A, B, C, D, E>(this GraphicScriptEditor editor, Color b)
        {
            EditorColors.Register<A>(b);
            EditorColors.Register<B>(b);
            EditorColors.Register<C>(b);
            EditorColors.Register<D>(b);
            EditorColors.Register<E>(b);
        }
        public static void Register<A, B, C, D, E,F>(this GraphicScriptEditor editor, Color b)
        {
            EditorColors.Register<A>(b);
            EditorColors.Register<B>(b);
            EditorColors.Register<C>(b);
            EditorColors.Register<D>(b);
            EditorColors.Register<E>(b);
            EditorColors.Register<F>(b);
        }
        public static void Register<A, B, C, D, E, F, G>(this GraphicScriptEditor editor, Color b)
        {
            EditorColors.Register<A>(b);
            EditorColors.Register<B>(b);
            EditorColors.Register<C>(b);
            EditorColors.Register<D>(b);
            EditorColors.Register<E>(b);
            EditorColors.Register<F>(b);
            EditorColors.Register<G>(b);
        }
        public static void Register<A, B, C, D, E, F, G, H>(this GraphicScriptEditor editor, Color b)
        {
            EditorColors.Register<A>(b);
            EditorColors.Register<B>(b);
            EditorColors.Register<C>(b);
            EditorColors.Register<D>(b);
            EditorColors.Register<E>(b);
            EditorColors.Register<F>(b);
            EditorColors.Register<G>(b);
            EditorColors.Register<H>(b);
        }
        public static void Register(this GraphicScriptEditor editor, Color b, Type t, params Type[] types)
        {
            EditorColors.Register(b, t);
            if (types != null)
                foreach (var tp in types)
                    EditorColors.Register(b, tp);
        }
        public static Color Get(this GraphicScriptEditor editor, Type t)
        {
            return EditorColors.Get(t);
        }
        public static Color Get<T>(this GraphicScriptEditor editor)
        {
            return EditorColors.Get<T>();
        }
    }
    class EditorColors
    {
        static Dictionary<Type, Color> colors = null;
        static void Load()
        {
            colors = new Dictionary<Type, Color>();
            Register<CommentStatement>(Colors.LightGray);
            Register((Color)ColorConverter.ConvertFromString("#59C059"), 
                typeof(NotExpression), typeof(BinaryExpression), typeof(ConditionalExpression),
                typeof(UpdateExpression));
            Register((Color)ColorConverter.ConvertFromString("#FFAB19"), typeof(LoopStatement),
                typeof(IfStatement), typeof(ForStatement), typeof(WhileStatement), typeof(BreakStatement), typeof(ContinueStatement),
                typeof(ReturnStatement), typeof(TryStatement),
                typeof(DoStatement));

            Register((Color)ColorConverter.ConvertFromString("#4C97FF"), typeof(ExpressionStatement));

            Register((Color)ColorConverter.ConvertFromString("#FF8C1A"),  typeof( AssignmentExpression),
                typeof(CallExpression));

            Register(Colors.DarkViolet, typeof(FunctionDeclaration),  typeof(VariableDeclarationExpression));

            Register(Colors.Green, typeof(Identifier));

            Register((Color)ColorConverter.ConvertFromString("#06cccc"), typeof(NewExpression), typeof(MemberExpression));

        }
        public static void Register<T>(Color b)
        {
            if (colors == null)
                Load();
            colors[typeof(T)] = b;
        }
        public static void Register(Color b, Type t)
        {
            if (colors == null)
                Load();
            colors[t] = b;
        }
        public static void Register(Color b, Type t, params Type[] types)
        {
            if (colors == null)
                Load();
            colors[t] = b;
            if (types != null)
                foreach (var tp in types)
                    colors[tp] = b;
        }
        public static Color Get<T>()
        {
            if (colors == null)
                Load();
            Type t = typeof(T);
            if (colors.ContainsKey(t))
                return colors[t];
            return Colors.Green;
        }
        public static Color Get(Type t)
        {
            if (colors == null)
                Load();
            if (colors.ContainsKey(t))
                return colors[t];
            return Colors.Green;
        }
    }
}
