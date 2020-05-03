using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Borto_v1
{
    /// <summary>
    /// Interaction logic for VideoPlayerPage.xaml
    /// </summary>
    public partial class VideoPlayerPage : Page
    {
        public VideoPlayerPage()
        {

            InitializeComponent();
            if (this.DataContext == null)
            this.DataContext = new VideoPlayerPageViewModel();

            Messenger.Default.Register<NotificationMessage>(
              this,
              message =>
              {
                  switch (message.Notification)
                  {
                      case "Choose":
                          {
                              OpenFileDialog openFileDialog = new OpenFileDialog();
                              openFileDialog.Filter = "Media files (*.mp3;*.mp4;*.mpg;*.mpeg)|*.mp3;*.mp4;*.mpg;*.mpeg|All files (*.*)|*.*";
                              if (openFileDialog.ShowDialog() == true)
                                  mePlayer.Source = new Uri(openFileDialog.FileName);
                              break;
                          }
                      case "Play":
                          {
                              mePlayer.Play();
                              break;
                          }
                      case "Pause":
                          {
                              mePlayer.Pause();
                              break;
                          }
                  }
              });
        }
    }
}
