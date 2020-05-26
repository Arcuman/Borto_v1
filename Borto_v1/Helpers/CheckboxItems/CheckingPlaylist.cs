using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borto_v1
{
    public class CheckingPlaylist
    {
        public bool isChecked { get; set; }

        public Playlist playlist { get; set; }

        public CheckingPlaylist(Playlist _playlist, bool _IsChecked)
        {
            playlist = _playlist;
            isChecked = _IsChecked;
        }
    }
}
