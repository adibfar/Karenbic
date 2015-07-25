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
        }
    }
}
