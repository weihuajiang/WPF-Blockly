using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ScratchNet
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void SetLanguage(string lang)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(lang);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(lang);
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            string language =IniFile.ReadValue("Language", "Language");
            if(!string.IsNullOrEmpty(language))
                SetLanguage(language);
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            base.OnStartup(e);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Unhandled exception found:\n" + e.ExceptionObject, "Unhandled exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                MessageBox.Show("Unhandled exception found:\n" + e.Exception.Message, "Unhandled exception", MessageBoxButton.OK, MessageBoxImage.Error);
            });
        }
    }
}
