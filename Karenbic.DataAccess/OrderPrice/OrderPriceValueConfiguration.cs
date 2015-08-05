using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class OrderPriceValueConfiguration : EntityTypeConfiguration<DomainClasses.OrderPriceValue>
    {
        public OrderPriceValueConfiguration()
        {
            HasRequired(x => x.OrderPrice)
                .WithMany(x => x.Values)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.Field)
                .WithMany(x => x.OrderPriceValues)
                .WillCascadeOnDelete(false);
        }
    }
}
