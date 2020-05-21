using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Borto_v1
{
    /// <summary>
    /// Interaction logic for AccountPage.xaml
    /// </summary>
    public partial class AccountPage : Page
    {
        public AccountPage()
        {
            InitializeComponent();
            this.DataContext = new AccountViewModel(SimpleIoc.Default.GetInstance<IFrameNavigationService>());
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
                                      AccountImage.ImageSource = new BitmapImage(new Uri(openFileDialog.FileName));
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
        private void AccountPage_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister(this);
        }
    }
}
