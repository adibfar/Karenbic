using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class DesignOrderConfiguration : EntityTypeConfiguration<DomainClasses.DesignOrder>
    {
        public DesignOrderConfiguration()
        {
            Property(x => x.Prepayment).HasPrecision(18, 0);

            HasOptional(x => x.PrepaymentFactor)
                .WithRequired(x => x.Order)
                .WillCascadeOnDelete(true);

            HasOptional(x => x.FinalFactor)
                .WithRequired(x => x.Order)
                .WillCascadeOnDelete(true);

            HasMany(x => x.Designs)
                .WithRequired(x => x.Order)
                .WillCascadeOnDelete(false);
        }
    }
}
