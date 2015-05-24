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
        }
    }
}
