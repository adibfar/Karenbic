using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class Order_Value_RadioButtonGroupConfiguration : EntityTypeConfiguration<DomainClasses.Order_Value_RadioButtonGroup>
    {
        public Order_Value_RadioButtonGroupConfiguration()
        {
            HasOptional(x => x.Value)
                .WithMany(x => x.Orders_Value)
                .WillCascadeOnDelete
                (false);
        }
    }
}
