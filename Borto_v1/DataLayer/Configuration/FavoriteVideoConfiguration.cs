using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borto_v1
{
    public class FavoriteVideoConfiguration : EntityTypeConfiguration<FavoriteVideo>
    {
        internal FavoriteVideoConfiguration() : base()
        {
            this.HasKey(p => p.IdFavoriteVideo);
            Property(p => p.IdFavoriteVideo).
                HasColumnName("IdFavoriteVideo").
                HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).
                IsRequired();
        }
    }
}
