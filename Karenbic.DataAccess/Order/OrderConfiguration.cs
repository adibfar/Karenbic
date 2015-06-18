using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class OrderConfiguration : EntityTypeConfiguration<DomainClasses.Order>
    {
        public OrderConfiguration()
        {
            Property(x => x.Price).HasPrecision(18, 0);

            HasRequired(x => x.Customer)
                .WithMany(x => x.Orders)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.Form)
                .WithMany(x => x.Orders)
                .WillCascadeOnDelete(false);

            HasMany(x => x.Values)
                .WithRequired(x => x.Order)
                .WillCascadeOnDelete(true);
        }
    }
}
