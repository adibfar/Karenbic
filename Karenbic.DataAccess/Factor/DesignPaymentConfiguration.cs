using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class DesignPaymentConfiguration : EntityTypeConfiguration<DomainClasses.DesignPayment>
    {
        public DesignPaymentConfiguration()
        {
            HasMany(x => x.PrepaymentItems)
                .WithRequired(x => x.Payment)
                .WillCascadeOnDelete(false);

            HasMany(x => x.FinalItems)
                .WithRequired(x => x.Payment)
                .WillCascadeOnDelete(false);

            HasMany(x => x.PrepaymentFactors)
                .WithOptional(x => x.Payment)
                .WillCascadeOnDelete(false);

            HasMany(x => x.FinalFactors)
                .WithOptional(x => x.Payment)
                .WillCascadeOnDelete(false);
        }
    }
}
