using Microsoft.Win32;
using System.Windows;

namespace LogWatcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            bool? result = fileDialog.ShowDialog();

            if (result == true)
            {
                WatchWindow.SetWatchFile(fileDialog.FileName);
            }
        }
    }
}
