using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class PrepaymentDesignFactorConfiguration : EntityTypeConfiguration<DomainClasses.PrepaymentDesignFactor>
    {
        public PrepaymentDesignFactorConfiguration()
        {
            HasRequired(x => x.Order)
                .WithOptional(x => x.PrepaymentFactor)
                .WillCascadeOnDelete(true);

            HasOptional(x => x.Payment)
               .WithMany(x => x.PrepaymentFactors)
               .WillCascadeOnDelete(false);
        }
    }
}
