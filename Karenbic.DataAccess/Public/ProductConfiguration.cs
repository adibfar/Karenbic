using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class ProductConfiguration : EntityTypeConfiguration<DomainClasses.Product>
    {
        public ProductConfiguration()
        {
            HasRequired(x => x.Category)
                .WithMany(x => x.Products)
                .WillCascadeOnDelete(false);

            HasMany(x => x.Pictures)
                .WithRequired(x => x.Product)
                .WillCascadeOnDelete(false);
        }
    }
}
