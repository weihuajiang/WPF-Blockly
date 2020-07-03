using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Lifetime.Clear;
using ToastNotifications.Position;
using ToastNotifications.Messages;
using ToastNotifications.Core;

namespace ScratchNet
{
    public class Toast : INotifyPropertyChanged
    {
        private readonly Notifier _notifier;

        public Toast()
        {
            _notifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(
                    parentWindow: Application.Current.MainWindow,
                    corner: Corner.BottomCenter,
                    offsetX: 0,
                    offsetY: 30);

                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(4),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(1));

                cfg.Dispatcher = Application.Current.Dispatcher;

                cfg.DisplayOptions.TopMost = false;
                cfg.DisplayOptions.Width = 450;
            });

            _notifier.ClearMessages(new ClearAll());
        }

        public void OnUnloaded()
        {
            _notifier.Dispose();
        }

        public void ShowInformation(string message)
        {
            _notifier.ShowInformation(message);
        }

        public void ShowInformation(string message, MessageOptions opts)
        {
            _notifier.ShowInformation(message, opts);
        }

        public void ShowSuccess(string message)
        {
            _notifier.ShowSuccess(message);
        }

        public void ShowSuccess(string message, MessageOptions opts)
        {
            _notifier.ShowSuccess(message, opts);
        }

        internal void ClearMessages(string msg)
        {
            _notifier.ClearMessages(new ClearByMessage(msg));
        }

        public void ShowWarning(string message, MessageOptions opts)
        {
            _notifier.ShowWarning(message, opts);
        }

        public void ShowError(string message)
        {
            _notifier.ShowError(message);
        }

        public void ShowError(string message, MessageOptions opts)
        {
            _notifier.ShowError(message, opts);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ClearAll()
        {
            _notifier.ClearMessages(new ClearAll());
        }
    }
}
