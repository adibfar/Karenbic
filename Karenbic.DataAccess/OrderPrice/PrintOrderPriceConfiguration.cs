using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class PrintOrderPriceConfiguration : EntityTypeConfiguration<DomainClasses.PrintOrderPrice>
    {
        public PrintOrderPriceConfiguration()
        {
            Property(x => x.PrintPrice).HasPrecision(18, 0);
            Property(x => x.PackingPrice).HasPrecision(18, 0);
        }
    }
}
