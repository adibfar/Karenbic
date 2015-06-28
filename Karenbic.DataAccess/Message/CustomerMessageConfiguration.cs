using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class CustomerMessageConfiguration : EntityTypeConfiguration<DomainClasses.CustomerMessage>
    {
        public CustomerMessageConfiguration()
        {
            HasRequired(x => x.Sender)
                .WithMany(x => x.Messages)
                .WillCascadeOnDelete(false);
        }
    }
}
