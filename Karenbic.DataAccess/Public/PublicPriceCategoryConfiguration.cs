using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class PublicPriceCategoryConfiguration : EntityTypeConfiguration<DomainClasses.PublicPriceCategory>
    {
        public PublicPriceCategoryConfiguration()
        {
            HasMany(x => x.Prices)
                .WithRequired(x => x.Category)
                .WillCascadeOnDelete(false);
        }
    }
}
