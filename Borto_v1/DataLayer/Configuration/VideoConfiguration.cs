
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Borto_v1
{
    public class VideoConfiguration : EntityTypeConfiguration<Video>
    {
        internal VideoConfiguration() : base()
        {
            this.HasKey(p => p.IdVideo);
            Property(p => p.IdVideo).
                HasColumnName("IdVideo").
                HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).
                IsRequired();


        }
    }
}
