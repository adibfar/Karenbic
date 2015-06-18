using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class PrintPaymentConfiguration : EntityTypeConfiguration<DomainClasses.PrintPayment>
    {
        public PrintPaymentConfiguration()
        {
            HasMany(x => x.Items)
                .WithRequired(x => x.Payment)
                .WillCascadeOnDelete(false);

            HasMany(x => x.Factors)
                .WithOptional(x => x.Payment)
                .WillCascadeOnDelete(false);
        }
    }
}
