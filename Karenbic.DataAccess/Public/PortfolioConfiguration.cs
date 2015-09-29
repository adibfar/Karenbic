using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class PortfolioConfiguration : EntityTypeConfiguration<DomainClasses.Portfolio>
    {
        public PortfolioConfiguration()
        {
            HasRequired(x => x.Category)
                .WithMany(x => x.Portfolios)
                .WillCascadeOnDelete(false);

            HasMany(x => x.Pictures)
                .WithRequired(x => x.Portfolio)
                .WillCascadeOnDelete(false);
        }
    }
}
