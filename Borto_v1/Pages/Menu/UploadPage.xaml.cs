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
    /// Interaction logic for UploadPage.xaml
    /// </summary>
    public partial class UploadPage : Page
    {
        public UploadPage()
        {
            InitializeComponent();

            Messenger.Default.Register<NotificationMessage>(
              this,
              message =>
              {
                  switch (message.Notification)
                  {
                      case "ChooseImage":
                          {
                              OpenFileDialog openFileDialog = new OpenFileDialog();
                              openFileDialog.Filter = "Image Files(*.jpg; *.jpeg; *.png; *.bmp) | *.jpg; *.jpeg; *.png; *.bmp";
                              if (openFileDialog.ShowDialog() == true)
                                  VideoImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                              break;
                          }
                      case "ChooseVideo":
                          {
                              OpenFileDialog openFileDialog = new OpenFileDialog();
                              openFileDialog.Filter = "Media files (*.mp3;*.mp4;*.mpg;*.mpeg)|*.mp3;*.mp4;*.mpg;*.mpeg|All files (*.*)|*.*";
                              if (openFileDialog.ShowDialog() == true)
                                  pathVideo.Text = new Uri(openFileDialog.FileName).OriginalString;
                              break;
                          }
                  }
              });
        }
    }
}
