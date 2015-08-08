using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class DesignOrderPriceConfiguration : EntityTypeConfiguration<DomainClasses.DesignOrderPrice>
    {
        public DesignOrderPriceConfiguration()
        {
            Property(x => x.Price).HasPrecision(18, 0);
            Property(x => x.Prepayment).HasPrecision(18, 0);
            Property(x => x.SpecialCreativityPrice).HasPrecision(18, 0);
            Property(x => x.SpecialCreativityPrepayment).HasPrecision(18, 0);
        }
    }
}
