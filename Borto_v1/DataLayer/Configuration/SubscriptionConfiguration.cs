using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borto_v1
{
   public class SubscriptionConfiguration : EntityTypeConfiguration<Subscription>
    {
        internal SubscriptionConfiguration() : base()
        {
            this.HasKey(p => p.SubscriptionId);
            Property(p => p.SubscriptionId).
                HasColumnName("SubscriptionId").
                HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).
                IsRequired();
            this.HasRequired(o => o.UserSub)
                .WithRequiredDependent()
               .WillCascadeOnDelete(false);
            this.HasRequired(o => o.UserSub)
         .WithMany(c => c.Subscriptions)
         .HasForeignKey(o => o.UserId);

        }
    }
}
