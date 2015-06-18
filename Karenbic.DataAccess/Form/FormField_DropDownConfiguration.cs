using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class FormField_DropDownConfiguration : EntityTypeConfiguration<DomainClasses.FormField_DropDown>
    {
        public FormField_DropDownConfiguration()
        {
            HasMany(x => x.Items)
                .WithRequired(x => x.DropDown)
                .WillCascadeOnDelete(true);
        }
    }
}
