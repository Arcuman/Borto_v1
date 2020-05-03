using Borto_v1.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borto_v1
{
    public class UploadViewModel : ViewModelBase
    {
        #region Private members 
        private IFrameNavigationService _navigationService;

        private Video video;

        private string image;

        #endregion

        #region Public members
        public string Image
        {
            get
            {
                return image;
            }
            set
            {
                if (image == value)
                {
                    return;
                }

                image = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Commands
        private RelayCommand _setPathtoVideoCommand;
        public RelayCommand SetPathtoVideoCommand
        {
            get
            {
                return _setPathtoVideoCommand
                    ?? (_setPathtoVideoCommand = new RelayCommand(
                    () =>
                    {
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        openFileDialog.Filter = "Media files (*.mp4)|*.mp4";
                        if (openFileDialog.ShowDialog() == true)
                             new Uri(openFileDialog.FileName);
                    }));
            }
        }
        #endregion
        #region ctor
        public UploadViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            video = new Video();
            Image = "/Assets/camera.jpg";
        }
        #endregion
    }

}
