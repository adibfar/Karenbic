using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class FormConfiguration : EntityTypeConfiguration<DomainClasses.Form>
    {
        public FormConfiguration()
        {
            HasMany(x => x.Fields)
                .WithRequired(x => x.Form)
                .WillCascadeOnDelete(true);
        }
    }
}
