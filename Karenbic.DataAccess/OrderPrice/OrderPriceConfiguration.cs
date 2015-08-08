using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class OrderPriceConfiguration : EntityTypeConfiguration<DomainClasses.OrderPrice>
    {
        public OrderPriceConfiguration()
        {
            HasRequired(x => x.Form)
                .WithMany(x => x.OrderPrices)
                .WillCascadeOnDelete(false);

            HasMany(x => x.Values)
                .WithRequired(x => x.OrderPrice)
                .WillCascadeOnDelete(false);
        }
    }
}
