using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class PortfolioPictureConfiguration : EntityTypeConfiguration<DomainClasses.PortfolioPicture>
    {
        public PortfolioPictureConfiguration()
        {
            HasRequired(x => x.Portfolio)
                .WithMany(x => x.Pictures)
                .WillCascadeOnDelete(false);
        }
    }
}
