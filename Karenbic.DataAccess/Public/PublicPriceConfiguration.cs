using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class PublicPriceConfiguration : EntityTypeConfiguration<DomainClasses.PublicPrice>
    {
        public PublicPriceConfiguration()
        {
            HasRequired(x => x.Category)
                .WithMany(x => x.Prices)
                .WillCascadeOnDelete(false);
        }
    }
}
