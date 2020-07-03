using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ScratchNet
{
    public abstract class ConsoleBaseStatement : Statement
    {
        internal static ConsoleWindow consoleWindow;
        public static ConsoleWindow GetConsole(ExecutionEnvironment enviroment)
        {
            if (!enviroment.HasValue("#$Console$#"))
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    consoleWindow = new ConsoleWindow();
                    consoleWindow.Owner = Application.Current.MainWindow;
                    consoleWindow.Show();
                });
                enviroment.GetBaseEnvironment().RegisterValue("#$Console$#", consoleWindow);
            }
            else
                consoleWindow = enviroment.GetValue("#$Console$#") as ConsoleWindow;
            return consoleWindow;
        }
        public static void Write(object obj)
        {
            if (consoleWindow != null)
                consoleWindow.Write(obj);
        }
        public static void WriteLine(object obj)
        {
            if (consoleWindow != null)
                consoleWindow.Write(obj + "\n");
        }
        public static void Stop()
        {
            if (consoleWindow != null)
            {
                consoleWindow.RequestStop();
            }
        }
    }
    public abstract class ConsoleBaseExpression : Expression
    {
        public static ConsoleWindow GetConsole(ExecutionEnvironment enviroment)
        {
            ConsoleWindow wnd = null;
            if (!enviroment.HasValue("#$Console$#"))
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    wnd = new ConsoleWindow();
                    wnd.Owner = Application.Current.MainWindow;
                    wnd.Show();
                });
                enviroment.GetBaseEnvironment().RegisterValue("#$Console$#", wnd);
                ConsoleBaseStatement.consoleWindow = wnd;
            }
            else
                wnd = enviroment.GetValue("#$Console$#") as ConsoleWindow;
            return wnd;
        }
    }
}
