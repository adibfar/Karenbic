using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class PortfolioTypeConfiguration : EntityTypeConfiguration<DomainClasses.PortfolioType>
    {
        public PortfolioTypeConfiguration()
        {
            HasMany(x => x.Categories)
                .WithRequired(x => x.Type)
                .WillCascadeOnDelete(false);
        }
    }
}
