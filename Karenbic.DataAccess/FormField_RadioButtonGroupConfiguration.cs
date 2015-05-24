using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class FormField_RadioButtonGroupConfiguration : EntityTypeConfiguration<DomainClasses.FormField_RadioButtonGroup>
    {
        public FormField_RadioButtonGroupConfiguration()
        {
            HasMany(x => x.Items)
                .WithRequired(x => x.RadioButtonGroup)
                .WillCascadeOnDelete(true);
        }
    }
}
