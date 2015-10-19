using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class SettingConfiguration : EntityTypeConfiguration<DomainClasses.Setting>
    {
        public SettingConfiguration()
        {
            Property(x => x.TransportPrice_BikeDelivery).HasPrecision(18, 0);
            Property(x => x.TransportPrice_Tipax).HasPrecision(18, 0);
            Property(x => x.TransportPrice_Porterage).HasPrecision(18, 0);
        }
    }
}
