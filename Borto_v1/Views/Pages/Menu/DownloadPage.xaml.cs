using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Borto_v1
{
    /// <summary>
    /// Interaction logic for DownloadPage.xaml
    /// </summary>
    public partial class DownloadPage : Page
    {
        public DownloadPage()
        {
            InitializeComponent();
            Messenger.Default.Register<NotificationMessage>(
              this,
              message =>
              {
                  switch (message.Notification)
                  {
                      case "ChooseFolder":
                          {
                              FolderBrowserDialog folderBrowser = new FolderBrowserDialog();

                              DialogResult result = folderBrowser.ShowDialog();

                              if (!string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
                              {
                                  pathFolder.Text = folderBrowser.SelectedPath;
                              }
                              break;
                          }
                  }
              });
        }

        private void DownloadPage_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister(this);
        }
    }
}
