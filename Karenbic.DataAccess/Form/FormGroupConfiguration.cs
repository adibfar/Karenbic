using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class FormGroupConfiguration : EntityTypeConfiguration<DomainClasses.FormGroup>
    {
        public FormGroupConfiguration()
        {
            HasMany(x => x.Forms)
                .WithRequired(x => x.Group)
                .WillCascadeOnDelete(false);
        }
    }
}
