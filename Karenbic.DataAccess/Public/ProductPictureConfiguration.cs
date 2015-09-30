using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class ProductPictureConfiguration : EntityTypeConfiguration<DomainClasses.ProductPicture>
    {
        public ProductPictureConfiguration()
        {
            HasRequired(x => x.Product)
                .WithMany(x => x.Pictures)
                .WillCascadeOnDelete(false);
        }
    }
}
