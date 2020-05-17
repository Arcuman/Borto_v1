using GalaSoft.MvvmLight.Ioc;
using System.Windows.Controls;
using System.Windows.Input;

namespace Borto_v1
{
    /// <summary>
    /// Interaction logic for VideoWatchingPage.xaml
    /// </summary>
    public partial class VideoWatchingPage : Page
    {
        public VideoWatchingPage()
        {
            this.DataContext = new VideoWatchingPageViewModel(SimpleIoc.Default.GetInstance<IFrameNavigationService>());
            InitializeComponent();

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
    }
}
