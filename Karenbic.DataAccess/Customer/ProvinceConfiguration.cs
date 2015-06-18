using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class ProvinceConfiguration : EntityTypeConfiguration<DomainClasses.Province>
    {
        public ProvinceConfiguration()
        {
            HasMany(x => x.Cities)
                .WithRequired(x => x.Province)
                .WillCascadeOnDelete(false);
        }
    }
}
