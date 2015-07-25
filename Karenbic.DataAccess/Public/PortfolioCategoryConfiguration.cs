using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class PortfolioCategoryConfiguration : EntityTypeConfiguration<DomainClasses.PortfolioCategory>
    {
        public PortfolioCategoryConfiguration()
        {
            HasRequired(x => x.Type)
                .WithMany(x => x.Categories)
                .WillCascadeOnDelete(false);

            HasMany(x => x.Portfolios)
                .WithRequired(x => x.Category)
                .WillCascadeOnDelete(false);
        }
    }
}
