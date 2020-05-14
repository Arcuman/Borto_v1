using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borto_v1
{
    public class CommentConfiguration : EntityTypeConfiguration<Comment>
    {
        internal CommentConfiguration() : base()
        {
            this.HasKey(p => p.IdComment);
            Property(p => p.IdComment).
                HasColumnName("IdComment").
                HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).
                IsRequired();
        }
    }
}
