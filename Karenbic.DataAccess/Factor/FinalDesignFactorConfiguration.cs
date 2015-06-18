using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class FinalDesignFactorConfiguration : EntityTypeConfiguration<DomainClasses.FinalDesignFactor>
    {
        public FinalDesignFactorConfiguration()
        {
            HasRequired(x => x.Order)
               .WithOptional(x => x.FinalFactor)
               .WillCascadeOnDelete(true);

            HasOptional(x => x.Payment)
               .WithMany(x => x.FinalFactors)
               .WillCascadeOnDelete(false);
        }
    }
}
