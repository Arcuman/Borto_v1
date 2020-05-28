using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borto_v1
{
    class NotificationConfiguration : EntityTypeConfiguration<Notification>
    {
        internal NotificationConfiguration() : base()
        {
            this.HasKey(p => p.NotificationId);
            Property(p => p.NotificationId).
                HasColumnName("NotificationId").
                HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).
                IsRequired();

            this.HasRequired(o => o.Sender)
                .WithRequiredDependent()
               .WillCascadeOnDelete(false);

            this.HasRequired(o => o.Sender)
         .WithMany(c => c.NotificationsSender)
         .HasForeignKey(o => o.SenderId);

            this.HasRequired(o => o.User)
                .WithRequiredDependent()
               .WillCascadeOnDelete(false);

            this.HasRequired(o => o.User)
         .WithMany(c => c.Notifications)
         .HasForeignKey(o => o.UserId); ;

        }
    }
}
