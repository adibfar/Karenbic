using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class AdminMessage_CustomerConfiguration : EntityTypeConfiguration<DomainClasses.AdminMessage_Customer>
    {
        public AdminMessage_CustomerConfiguration()
        {
            HasRequired(x => x.Customer)
                .WithMany(x => x.AdminMessages)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.AdminMessage_Admin)
                .WithMany(x => x.AdminMessages_Customer)
                .WillCascadeOnDelete(false);
        }
    }
}
