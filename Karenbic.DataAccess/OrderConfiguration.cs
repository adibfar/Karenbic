using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class OrderConfiguration : EntityTypeConfiguration<DomainClasses.Order>
    {
        public OrderConfiguration()
        {
            HasRequired(x => x.Form)
                .WithMany(x => x.Orders)
                .WillCascadeOnDelete(false);

            HasMany(x => x.Values)
                .WithRequired(x => x.Order)
                .WillCascadeOnDelete(true);
        }
    }
}
