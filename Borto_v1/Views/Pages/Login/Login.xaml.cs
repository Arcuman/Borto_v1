using GalaSoft.MvvmLight.Ioc;
using System.Windows.Controls;

namespace Borto_v1
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
            this.DataContext = new LoginViewModel(SimpleIoc.Default.GetInstance<IFrameNavigationService>());
        }

    }
}
