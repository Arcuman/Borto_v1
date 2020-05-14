using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace Borto_v1
{
    class MarkConfiguration : EntityTypeConfiguration<Mark>
    {
        internal MarkConfiguration() : base()
        {
            this.HasKey(p => p.IdMark);
            Property(p => p.IdMark).
                HasColumnName("IdMark").
                HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).
                IsRequired();
        }
    }
}
