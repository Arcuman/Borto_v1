using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Borto_v1
{
    public class UpdateViewModel : ICommand
    {
        private IChangeViewModel viewModel;

        public UpdateViewModel(IChangeViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter.ToString() == "Watching")
            {
                viewModel.SelectedViewModel = SimpleIoc.Default.GetInstance<WatchingViewModel>();
            }
            else if (parameter.ToString() == "Account")
            {
                viewModel.SelectedViewModel = new AccountViewModel();
            } 
            else if (parameter.ToString() == "Settings")
            {
                viewModel.SelectedViewModel = new SettingsViewModel();
            } 
            else if (parameter.ToString() == "Upload")
            {
                viewModel.SelectedViewModel = new UploadViewModel();
            }
            else if (parameter.ToString() == "Download")
            {
                viewModel.SelectedViewModel = new DownloadViewModel();
            }
            else if (parameter.ToString() == "Login")
            {
                viewModel.SelectedViewModel = new LoginViewModel();
            }
            else if (parameter.ToString() == "Registration")
            {
                viewModel.SelectedViewModel = new RegisterViewModel();
            }
            else if (parameter.ToString() == "Player")
            {
                viewModel.SelectedViewModel = new VideoPlayerPageViewModel();
            }
            else if (parameter.ToString() == "OnlinePlayer")
            {
                viewModel.SelectedViewModel = new VideoWatchingPageViewModel();
            }

        }
    }
}
