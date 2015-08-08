using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class OrderPriceValue_DropDownConfiguration : EntityTypeConfiguration<DomainClasses.OrderPriceValue_DropDown>
    {
        public OrderPriceValue_DropDownConfiguration()
        {
            HasOptional(x => x.Value)
                .WithMany(x => x.OrderPriceValues)
                .WillCascadeOnDelete(false);
        }
    }
}
