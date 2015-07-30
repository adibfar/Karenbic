using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class FinancialConflictItemConfiguration : EntityTypeConfiguration<DomainClasses.FinancialConflictItem>
    {
        public FinancialConflictItemConfiguration()
        {
            Property(x => x.Price).HasPrecision(18, 0);

            HasRequired(x => x.FinancialConflict)
                .WithMany(x => x.Items)
                .WillCascadeOnDelete(true);
        }
    }
}
