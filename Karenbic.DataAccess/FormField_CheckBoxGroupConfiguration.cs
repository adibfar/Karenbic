using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class FormField_CheckBoxGroupConfiguration : EntityTypeConfiguration<DomainClasses.FormField_CheckBoxGroup>
    {
        public FormField_CheckBoxGroupConfiguration()
        {
            HasMany(x => x.Items)
                .WithRequired(x => x.CheckBoxGroup)
                .WillCascadeOnDelete(true);
        }
    }
}
