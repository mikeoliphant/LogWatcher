using System;
using System.IO;
using System.Printing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace LogWatcher
{
    /// <summary>
    /// Interaction logic for WatcherWindow.xaml
    /// </summary>
    public partial class WatcherWindow : UserControl
    {
        FileWatcher watcher;

        bool seekToEnd = true;
        bool isUserScroll = true;

        public WatcherWindow()
        {
            InitializeComponent();

            this.Loaded += WatcherWindow_Loaded;
        }

        private void WatcherWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Closing += WatcherWindow_Closing;
        }

        private void WatcherWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (watcher != null)
            {
                watcher.Stop();
            }
        }

        public void SetWatchFile(string path)
        {
            if (watcher != null)
                watcher.Stop();

            watcher = new FileWatcher(path);
            watcher.UpdateAction = delegate (string text)
            {
                Dispatcher.Invoke(delegate
                {
                    bool doAutoScroll = false;

                    if (LogScroller.VerticalOffset == LogScroller.ScrollableHeight)
                    {
                        doAutoScroll = true;
                    }

                    LogTextBox.AppendText(text);

                    if (doAutoScroll)
                    {
                        isUserScroll = false;
                        LogScroller.ScrollToEnd();
                    }
                });
            };
            watcher.Start();
        }
    }
}
