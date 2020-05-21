using GalaSoft.MvvmLight.Ioc;
using System.Windows.Controls;
using System.Windows.Input;

namespace Borto_v1
{
    /// <summary>
    /// Interaction logic for WatchingPage.xaml
    /// </summary>
    public partial class WatchingPage : Page
    {
        public WatchingPage()
        {
            InitializeComponent();
            this.DataContext = new WatchingViewModel(SimpleIoc.Default.GetInstance<IFrameNavigationService>());
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
