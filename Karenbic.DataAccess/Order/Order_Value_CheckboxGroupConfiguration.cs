using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class Order_Value_CheckboxGroupConfiguration : EntityTypeConfiguration<DomainClasses.Order_Value_CheckboxGroup>
    {
        public Order_Value_CheckboxGroupConfiguration()
        {
            HasMany(x => x.Values)
                .WithMany(x => x.Orders_Value)
                .Map(x =>
                {
                    x.ToTable("tbl_Order_Value_CheckBoxGroup_Items");
                    x.MapLeftKey("FormField_CheckBoxGroup_Item_Id");
                    x.MapRightKey("Order_Value_CheckBoxGroup_Id");
                });
        }
    }
}
