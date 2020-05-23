using GalaSoft.MvvmLight.Ioc;
using System.Windows.Controls;

namespace Borto_v1
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
            this.DataContext = new RegisterViewModel(SimpleIoc.Default.GetInstance<IFrameNavigationService>());
        }
    }
}
