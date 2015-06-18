using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class PrepaymentDesignPaymentItemConfiguration : EntityTypeConfiguration<DomainClasses.PrepaymentDesignPaymentItem>
    {
        public PrepaymentDesignPaymentItemConfiguration()
        {
            HasRequired(x => x.Payment)
                .WithMany(x => x.PrepaymentItems)
                .WillCascadeOnDelete(false);
        }
    }
}
