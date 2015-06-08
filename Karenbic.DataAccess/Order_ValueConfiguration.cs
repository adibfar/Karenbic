using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class Order_ValueConfiguration : EntityTypeConfiguration<DomainClasses.Order_Value>
    {
        public Order_ValueConfiguration()
        {
            HasRequired(x => x.Order)
                .WithMany(x => x.Values)
                .WillCascadeOnDelete(true);

            HasRequired(x => x.Field)
                .WithMany(x => x.Orders_Value)
                .WillCascadeOnDelete(false);
        }
    }
}
