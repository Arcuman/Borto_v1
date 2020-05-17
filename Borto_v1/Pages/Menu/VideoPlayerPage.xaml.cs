using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;

namespace Borto_v1
{
    /// <summary>
    /// Interaction logic for VideoPlayerPage.xaml
    /// </summary>
    public partial class VideoPlayerPage : Page
    {
        bool userIsDraggingSlider = false;
        DispatcherTimer timer;
        public VideoPlayerPage()
        {

            InitializeComponent();
            if (this.DataContext == null)
            this.DataContext = new VideoPlayerPageViewModel();

             timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
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
                      case "FullScreen":
                          {
                              grid_player.SetValue(Grid.RowSpanProperty, 2);
                              grid_content.SetValue(Grid.RowProperty, 0);
                              grid_content.SetValue(Grid.RowSpanProperty, 2);
                              break;
                          }
                      case "NotFullScreen":
                          {
                              grid_player.SetValue(Grid.RowSpanProperty, 1); 
                              grid_content.SetValue(Grid.RowProperty, 1);
                              grid_content.SetValue(Grid.RowSpanProperty, 1);
                              break;
                          }
                  }
              });
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if ((mePlayer.Source != null) && (mePlayer.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider))
            {
                sliProgress.Minimum = 0;
                sliProgress.Maximum = mePlayer.NaturalDuration.TimeSpan.TotalSeconds;
                sliProgress.Value = mePlayer.Position.TotalSeconds;
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister(this);
            timer = null;
        }

        private void sliProgress_DragStarted(object sender, DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;
        }

        private void sliProgress_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            mePlayer.Position = TimeSpan.FromSeconds(sliProgress.Value);
        }

        private void sliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblProgressStatus.Text = TimeSpan.FromSeconds(sliProgress.Value).ToString(@"hh\:mm\:ss");
        }

        private void focuselement_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = false;
        }

        private void focuselement_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {
                mePlayer.Position += TimeSpan.FromSeconds(5);
                e.Handled = true;
            }
            else if (e.Key == Key.Left)
            {
                mePlayer.Position -= TimeSpan.FromSeconds(5);
                e.Handled = true;
            }

        }

    }
}
