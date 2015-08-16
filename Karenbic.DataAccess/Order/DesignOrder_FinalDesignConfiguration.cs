using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class DesignOrder_FinalDesignConfiguration : EntityTypeConfiguration<DomainClasses.DesignOrder_FinalDesign>
    {
        public DesignOrder_FinalDesignConfiguration()
        {
            HasRequired(x => x.Order)
                .WithMany(x => x.FinalDesigns)
                .WillCascadeOnDelete(false);
        }
    }
}
