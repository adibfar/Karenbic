using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class FinancialConflictConfiguration : EntityTypeConfiguration<DomainClasses.FinancialConflict>
    {
        public FinancialConflictConfiguration()
        {
            Property(x => x.Price).HasPrecision(18, 0);

            HasRequired(x => x.Customer)
                .WithMany(x => x.FinancialConflicts)
                .WillCascadeOnDelete(false);

            HasMany(x => x.Items)
                .WithRequired(x => x.FinancialConflict)
                .WillCascadeOnDelete(true);
        }
    }
}
