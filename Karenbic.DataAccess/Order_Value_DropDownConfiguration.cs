using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class Order_Value_DropDownConfiguration : EntityTypeConfiguration<DomainClasses.Order_Value_DropDown>
    {
        public Order_Value_DropDownConfiguration()
        {
            HasOptional(x => x.Value)
                .WithMany(x => x.Orders_Value)
                .WillCascadeOnDelete(false);
        }
    }
}
