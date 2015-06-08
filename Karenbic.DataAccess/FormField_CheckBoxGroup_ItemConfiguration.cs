using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class FormField_CheckBoxGroup_ItemConfiguration : EntityTypeConfiguration<DomainClasses.FormField_CheckBoxGroup_Item>
    {
        public FormField_CheckBoxGroup_ItemConfiguration()
        {
            HasRequired(x => x.CheckBoxGroup)
                .WithMany(x => x.Items)
                .WillCascadeOnDelete(true);

            HasMany(x => x.Orders_Value)
                .WithMany(x => x.Values)
                .Map(x =>
                {
                    x.ToTable("tbl_Order_Value_CheckBoxGroup_Items");
                    x.MapLeftKey("Order_Value_CheckBoxGroup_Id");
                    x.MapRightKey("FormField_CheckBoxGroup_Item_Id");
                });
        }
    }
}
