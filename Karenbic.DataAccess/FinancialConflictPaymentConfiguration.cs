using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class FinancialConflictPaymentConfiguration : EntityTypeConfiguration<DomainClasses.FinancialConflictPayment>
    {
        public FinancialConflictPaymentConfiguration()
        {
            HasRequired(x => x.FinancialConflict)
                .WithMany(x => x.Payments)
                .WillCascadeOnDelete(false);
        }
    }
}
