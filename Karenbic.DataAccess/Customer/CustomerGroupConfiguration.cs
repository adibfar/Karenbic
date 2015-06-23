using System.Data.Entity.ModelConfiguration;
namespace Karenbic.DataAccess
{
    class CustomerGroupConfiguration : EntityTypeConfiguration<DomainClasses.CustomerGroup>
    {
        public CustomerGroupConfiguration()
        {
            HasMany(x => x.Customers)
                .WithOptional(x => x.Group)
                .WillCascadeOnDelete(false);
        }
    }
}
