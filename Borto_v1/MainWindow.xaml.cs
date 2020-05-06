using Borto_v1;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace Borto_v1
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool MenuCLosed = false;
        public MainWindow()
        {
            InitializeComponent();
            Messenger.Default.Register<NotificationMessage>(
             this,
             message =>
             {
                 switch (message.Notification)
                 {
                     case "FullScreen":
                         {
                             this.WindowStyle = WindowStyle.None;
                             this.WindowState = WindowState.Maximized;
                             content.SetValue(Grid.ColumnProperty, 0);
                             content.SetValue(Grid.ColumnSpanProperty, 2);
                             break;
                         }
                     case "NotFullScreen":
                         {
                             this.WindowState = WindowState.Normal;
                             this.WindowStyle = WindowStyle.SingleBorderWindow;
                             content.SetValue(Grid.ColumnProperty, 1);
                             content.SetValue(Grid.ColumnSpanProperty, 1);
                             break;
                         }

                 }
             });
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (MenuCLosed)
            {
                Storyboard openMenu = (Storyboard)button.FindResource("OpenMenu");
                openMenu.Begin();
            }
            else
            {
                Storyboard closeMenu = (Storyboard)button.FindResource("CloseMenu");
                closeMenu.Begin();
            }
            MenuCLosed = !MenuCLosed;
        }

        private void BortoWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister(this);
        }
    }
}
