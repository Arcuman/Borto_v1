using GalaSoft.MvvmLight.Ioc;
using System.Windows.Controls;

namespace Borto_v1
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();

            this.DataContext = new AdminViewModel(SimpleIoc.Default.GetInstance<IFrameNavigationService>());
        }
    }
}
