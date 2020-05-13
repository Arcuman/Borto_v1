using GalaSoft.MvvmLight.Ioc;
using System.Windows.Controls;

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
    }
}
