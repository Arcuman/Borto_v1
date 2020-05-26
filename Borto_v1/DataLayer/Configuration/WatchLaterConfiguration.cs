using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borto_v1
{
    public class WatchLaterConfiguration : EntityTypeConfiguration<WatchLater>
    {
        internal WatchLaterConfiguration() : base()
        {
            this.HasKey(p => p.IdWatchLater);
            Property(p => p.IdWatchLater).
                HasColumnName("IdWatchLater").
                HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).
                IsRequired();
        }
    }
}
