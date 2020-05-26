using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borto_v1
{
    public class Playlist
    {
        public int IdPlaylist { get; set; }

        [Required]
        [MaxLength(25)]
        public string Name { get; set; }


        public byte[] Image { get; set; }

        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public virtual List<PlaylistVideo> PlaylistVideos { get; set; }

        public Playlist()
        {

        }
    }
}
