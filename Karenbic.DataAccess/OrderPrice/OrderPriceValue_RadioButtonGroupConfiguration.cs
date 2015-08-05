using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class OrderPriceValue_RadioButtonGroupConfiguration : EntityTypeConfiguration<DomainClasses.OrderPriceValue_RadioButtonGroup>
    {
        public OrderPriceValue_RadioButtonGroupConfiguration()
        {
            HasOptional(x => x.Value)
                .WithMany(x => x.OrderPriceValues)
                .WillCascadeOnDelete
                (false);
        }
    }
}
