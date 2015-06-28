using System.Data.Entity.ModelConfiguration;
namespace Karenbic.DataAccess
{
    class CustomerConfiguration : EntityTypeConfiguration<DomainClasses.Customer>
    {
        public CustomerConfiguration()
        {
            HasRequired(x => x.City)
                .WithMany(x => x.Customers)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.Group)
                .WithMany(x => x.Customers)
                .WillCascadeOnDelete(false);

            HasMany(x => x.Orders)
                .WithRequired(x => x.Customer)
                .WillCascadeOnDelete(false);

            HasMany(x => x.Messages)
                .WithRequired(x => x.Sender)
                .WillCascadeOnDelete(false);

            HasMany(x => x.AdminMessages)
                .WithRequired(x => x.Customer)
                .WillCascadeOnDelete(false);

            HasMany(x => x.AdminMessages_Admin)
                .WithMany(x => x.Customers)
                .Map(x =>
                {
                    x.ToTable("tbl_AdminMessages_Admin_Customer");
                    x.MapLeftKey("AdminMessages_Admin_Id");
                    x.MapRightKey("Customer_Id");
                });
        }
    }
}
