using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class PaymentConfiguration : EntityTypeConfiguration<DomainClasses.Payment>
    {
        public PaymentConfiguration()
        {
            Property(x => x.Money).HasPrecision(18, 0);
        }
    }
}
