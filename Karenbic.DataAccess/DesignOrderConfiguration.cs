using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class DesignOrderConfiguration : EntityTypeConfiguration<DomainClasses.DesignOrder>
    {
        public DesignOrderConfiguration()
        {
            Property(x => x.Prepayment).HasPrecision(18, 0);
        }
    }
}
