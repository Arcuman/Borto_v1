using Borto_v1.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borto_v1.ViewModel
{
    public class UploadViewModel : ViewModelBase
    {
        #region Private members 
        private IFrameNavigationService _navigationService;

        private Video video;

        #endregion

        #region Public members
        public Video SelectedVideo
        {
            get
            {
                return video;
            }
            set
            {
                if (video == value)
                {
                    return;
                }

                video = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Commands
        private RelayCommandParametr _uploadpageCommand;
        public RelayCommandParametr UploadPageCommand
        {
            get
            {
                return _uploadpageCommand
                    ?? (_uploadpageCommand = new RelayCommandParametr(
                    obj =>
                    {
                        SelectedVideo = obj as Video;
                    }));
            }
        }
        #endregion
        #region ctor
        public UploadViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        #endregion
    }

}
