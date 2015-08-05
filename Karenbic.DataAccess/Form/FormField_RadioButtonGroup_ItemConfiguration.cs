using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class FormField_RadioButtonGroup_ItemConfiguration : EntityTypeConfiguration<DomainClasses.FormField_RadioButtonGroup_Item>
    {
        public FormField_RadioButtonGroup_ItemConfiguration()
        {
            HasRequired(x => x.RadioButtonGroup)
                .WithMany(x => x.Items)
                .WillCascadeOnDelete(true);

            HasMany(x => x.OrderPriceValues)
                .WithOptional(x => x.Value)
                .WillCascadeOnDelete(false);

            HasMany(x => x.Orders_Value)
                .WithOptional(x => x.Value)
                .WillCascadeOnDelete(false);
        }
    }
}
