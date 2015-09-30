using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class ProductCategoryConfiguration : EntityTypeConfiguration<DomainClasses.ProductCategory>
    {
        public ProductCategoryConfiguration()
        {
            HasMany(x => x.Products)
                .WithRequired(x => x.Category)
                .WillCascadeOnDelete(false);
        }
    }
}
