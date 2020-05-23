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
    /// Interaction logic for AdminVideosPage.xaml
    /// </summary>
    public partial class AdminVideosPage : Page
    {
        public AdminVideosPage()
        {
            InitializeComponent();

            this.DataContext = new AdminVideosViewModel();
        }
    }
}
