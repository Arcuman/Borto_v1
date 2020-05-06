using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borto_v1
{
    public interface IChangeViewModel
    {
        ViewModelBase SelectedViewModel { get; set; }
    }
}
