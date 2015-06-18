using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class CityConfiguration : EntityTypeConfiguration<DomainClasses.City>
    {
        public CityConfiguration()
        {
            HasRequired(x => x.Province)
                .WithMany(x => x.Cities)
                .WillCascadeOnDelete(false);

            HasMany(x => x.Customers)
                .WithRequired(x => x.City)
                .WillCascadeOnDelete(false);
        }
    }
}
