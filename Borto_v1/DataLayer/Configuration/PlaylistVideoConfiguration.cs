using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borto_v1
{
    public class PlaylistVideoConfiguration : EntityTypeConfiguration<PlaylistVideo>
    {
        internal PlaylistVideoConfiguration() : base()
        {
            this.HasKey(p => p.IdPlaylistVideo);
            Property(p => p.IdPlaylistVideo).
                HasColumnName("IdPlaylistVideo").
                HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).
                IsRequired();
        }
    }
}
