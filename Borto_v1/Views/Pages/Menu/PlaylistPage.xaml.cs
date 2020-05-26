using GalaSoft.MvvmLight.Ioc;
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
    /// Interaction logic for PlaylistPage.xaml
    /// </summary>
    public partial class PlaylistPage : Page
    {
        public PlaylistPage()
        {
            InitializeComponent();
            this.DataContext = new PlaylistViewModel(SimpleIoc.Default.GetInstance<IFrameNavigationService>());
            Messenger.Default.Register<NotificationMessage>(
              this,
              message =>
              {
                  try
                  {
                      switch (message.Notification)
                      {

                          case "ChooseImage":
                              {
                                  OpenFileDialog openFileDialog = new OpenFileDialog();
                                  openFileDialog.Filter = "Image Files(*.jpg; *.jpeg; *.png; *.bmp) | *.jpg; *.jpeg; *.png; *.bmp";
                                  if (openFileDialog.ShowDialog() == true)
                                      PlaylistImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                                  break;
                              }
                      }
                  }
                  catch (ArgumentException ex)
                  {
                  }
              });
        }
        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollViewer = (ScrollViewer)sender;
            if (e.Delta < 0)
            {
                scrollViewer.LineDown();
            }
            else
            {
                scrollViewer.LineUp();
            }
            e.Handled = true;
        }
        private void PlaylistPage_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister(this);
        }
    }
}
