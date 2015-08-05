using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class OrderPriceValue_CheckboxGroupConfiguration : EntityTypeConfiguration<DomainClasses.OrderPriceValue_CheckboxGroup>
    {
        public OrderPriceValue_CheckboxGroupConfiguration()
        {
            HasMany(x => x.Values)
                .WithMany(x => x.OrderPriceValues)
                .Map(x =>
                {
                    x.ToTable("tbl_OrderPriceValue_CheckBoxGroup_Items");
                    x.MapLeftKey("FormField_CheckBoxGroup_Item_Id");
                    x.MapRightKey("OrderPriceValue_CheckBoxGroup_Id");
                });
        }
    }
}
