using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ScratchNet
{
    /// <summary>
    /// Interaction logic for ConsoleWindow.xaml
    /// </summary>
    public partial class ConsoleWindow : Window
    {
        public ConsoleWindow()
        {
            InitializeComponent();
            ConsoleTextBox.Focus();
        }

        StringBuilder stringBuilder = new StringBuilder();
        public void Clear()
        {
            if (Dispatcher.Thread.ManagedThreadId == Thread.CurrentThread.ManagedThreadId)
            {
                stringBuilder.Clear();
                ConsoleTextBox.Clear();
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    stringBuilder.Clear();
                    ConsoleTextBox.Clear();
                });
            }
        }
        void Append(object obj)
        {
            ConsoleTextBox.AppendText(obj + "");
            ConsoleTextBox.ScrollToEnd();
            ConsoleTextBox.CaretIndex = ConsoleTextBox.Text.Length;
        }
        public void Write(object obj)
        {
            if(Dispatcher.Thread.ManagedThreadId==Thread.CurrentThread.ManagedThreadId)
                Append(obj + "");
            else
            {
                Dispatcher.Invoke(() =>
                {
                    Append(obj + "");
                });
            }
        }
        bool canInput = false;
        StringBuilder sb = new StringBuilder();
        int maxLength = 0;
        bool isLine = false;
        bool isStoped=false;
        internal AutoResetEvent waitEvent = new AutoResetEvent(false);
        public void RequestStop()
        {
            sb.Clear();
            canInput = true;
            maxLength = 1;
            isLine = false;
            isStoped = true;
            waitEvent.Set();
        }
        public string Read()
        {
            Dispatcher.Invoke(() =>
            {
                ConsoleTextBox.IsReadOnlyCaretVisible = true;
            });
            sb.Clear();
            canInput = true;
            maxLength = 1;
            isLine = false;
            waitEvent.WaitOne();
            canInput = false;
            maxLength = 0;
            isLine = false;
            Dispatcher.Invoke(() =>
            {
                ConsoleTextBox.IsReadOnlyCaretVisible = false;
            });
            return sb.ToString();
        }
        public string ReadLine()
        {
            Dispatcher.Invoke(() =>
            {
                ConsoleTextBox.IsReadOnlyCaretVisible = true;
            });
            sb.Clear();
            canInput = true;
            maxLength = 0;
            isLine = true;
            waitEvent.WaitOne();
            canInput = false;
            maxLength = 0;
            isLine = false;
            Dispatcher.Invoke(() =>
            {
                ConsoleTextBox.IsReadOnlyCaretVisible = false;
            });
            return sb.ToString();
        }
        private void ConsolePreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (isStoped)
            {
                e.Handled = true;
                this.Close();
            }
            if (canInput)
            {
                Append(e.Text);
                ConsoleTextBox.CaretIndex = ConsoleTextBox.Text.Length;
                if (e.Text.Length==1 && (int)(e.Text[0])==13)
                {
                    e.Handled = true;
                    waitEvent.Set();
                    return;
                }
                sb.Append(e.Text);
                if (maxLength>0 && sb.Length >= maxLength)
                {
                    canInput = false;
                    waitEvent.Set();
                }
                if(isLine && e.Text == "\n")
                {
                    canInput = false;
                    waitEvent.Set();
                }
            }
            e.Handled = true;
        }

        private void ConsoleTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (isStoped)
            {
                e.Handled = true;
                this.Close();
            }
            if (canInput)
            {
                if (maxLength > 0 && sb.Length >= maxLength)
                {
                    canInput = false;
                    waitEvent.Set();
                }
                if (e.Key == Key.Space)
                {
                    Append(" ");
                    sb.Append(" ");
                }
                else if (e.Key == Key.Tab)
                {
                    Append("\t");
                    sb.Append("\t");
                }
                else if(e.Key== Key.Back)
                {
                    if (sb.Length >= 1)
                    {
                        ConsoleTextBox.Text = ConsoleTextBox.Text.Substring(0, ConsoleTextBox.Text.Length - 1);
                        ConsoleTextBox.CaretIndex = ConsoleTextBox.Text.Length;
                        sb.Remove(sb.Length - 1, 1);
                    }
                }
            }
            e.Handled = false;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (isStoped)
            {
                e.Handled = true;
                this.Close();
            }
        }

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                ConsoleTextBox.ScrollToVerticalOffset(ConsoleTextBox.VerticalOffset + 1);
            }
            else
                ConsoleTextBox.ScrollToVerticalOffset(ConsoleTextBox.VerticalOffset - 1);
        }

        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                //ConsoleTextBox.ScrollToVerticalOffset(ConsoleTextBox.VerticalOffset + 1);
                ConsoleTextBox.LineUp();
            }
            else
                ConsoleTextBox.LineDown();
            // ConsoleTextBox.ScrollToVerticalOffset(ConsoleTextBox.VerticalOffset - 1);
            e.Handled = true;
        }
    }
}
