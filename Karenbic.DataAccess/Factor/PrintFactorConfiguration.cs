using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class PrintFactorConfiguration : EntityTypeConfiguration<DomainClasses.PrintFactor>
    {
        public PrintFactorConfiguration()
        {
            HasRequired(x => x.Order)
                .WithOptional(x => x.Factor)
                .WillCascadeOnDelete(true);

            HasOptional(x => x.Payment)
                .WithMany(x => x.Factors)
                .WillCascadeOnDelete(false);
        }
    }
}
