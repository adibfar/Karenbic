using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class FormConfiguration : EntityTypeConfiguration<DomainClasses.Form>
    {
        public FormConfiguration()
        {
            HasRequired(x => x.Group)
                .WithMany(x => x.Forms)
                .WillCascadeOnDelete(false);

            HasMany(x => x.Fields)
                .WithRequired(x => x.Form)
                .WillCascadeOnDelete(true);

            HasMany(x => x.OrderPrices)
                .WithRequired(x => x.Form)
                .WillCascadeOnDelete(false);

            HasMany(x => x.Orders)
                .WithRequired(x => x.Form)
                .WillCascadeOnDelete(false);
        }
    }
}
