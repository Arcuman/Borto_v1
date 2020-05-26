using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace Borto_v1
{
    public class PlaylistConfiguration : EntityTypeConfiguration<Playlist>
    {
        internal PlaylistConfiguration() : base()
        {
            this.HasKey(p => p.IdPlaylist);
            Property(p => p.IdPlaylist).
                HasColumnName("IdPlaylist").
                HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).
                IsRequired();
        }

    }
}
