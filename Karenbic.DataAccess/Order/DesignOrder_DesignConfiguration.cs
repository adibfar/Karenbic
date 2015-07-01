using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class DesignOrder_DesignConfiguration : EntityTypeConfiguration<DomainClasses.DesignOrder_Design>
    {
        public DesignOrder_DesignConfiguration()
        {
            HasRequired(x => x.Order)
                .WithMany(x => x.Designs)
                .WillCascadeOnDelete(false);

            HasMany(x => x.Files)
                .WithRequired(x => x.Design)
                .WillCascadeOnDelete(false);
        }
    }
}
