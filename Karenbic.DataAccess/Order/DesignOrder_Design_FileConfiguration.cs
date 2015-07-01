using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class DesignOrder_Design_FileConfiguration : EntityTypeConfiguration<DomainClasses.DesignOrder_Design_File>
    {
        public DesignOrder_Design_FileConfiguration()
        {
            HasRequired(x => x.Design)
                .WithMany(x => x.Files)
                .WillCascadeOnDelete(false);
        }
    }
}
