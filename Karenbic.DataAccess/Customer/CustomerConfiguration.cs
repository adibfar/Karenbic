using System.Data.Entity.ModelConfiguration;
namespace Karenbic.DataAccess
{
    class CustomerConfiguration : EntityTypeConfiguration<DomainClasses.Customer>
    {
        public CustomerConfiguration()
        {
            HasRequired(x => x.City)
                .WithMany(x => x.Customers)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.Group)
                .WithMany(x => x.Customers)
                .WillCascadeOnDelete(false);

            HasMany(x => x.Orders)
                .WithRequired(x => x.Customer)
                .WillCascadeOnDelete(false);
        }
    }
}
