
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Borto_v1
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        internal UserConfiguration() : base()
        {
            this.HasKey(p => p.IdUser);
            Property(p => p.IdUser).
                HasColumnName("IdUser").
                HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).
                IsRequired();
        }

    }
}
