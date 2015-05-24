using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class FormField_DropDown_ItemConfiguration : EntityTypeConfiguration<DomainClasses.FormField_DropDown_Item>
    {
        public FormField_DropDown_ItemConfiguration()
        {
            HasRequired(x => x.DropDown)
                .WithMany(x => x.Items)
                .WillCascadeOnDelete(true);
        }
    }
}
