using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class PrintOrderConfiguration : EntityTypeConfiguration<DomainClasses.PrintOrder>
    {
        public PrintOrderConfiguration()
        {
            Property(x => x.PrintPrice).HasPrecision(18, 0);
            Property(x => x.PackingPrice).HasPrecision(18, 0);

            HasOptional(x => x.Factor)
                .WithRequired(x => x.Order)
                .WillCascadeOnDelete(true);
        }
    }
}
