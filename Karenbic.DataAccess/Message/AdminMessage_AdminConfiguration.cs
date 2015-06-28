using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class AdminMessage_AdminConfiguration : EntityTypeConfiguration<DomainClasses.AdminMessage_Admin>
    {
        public AdminMessage_AdminConfiguration()
        {
            HasMany(x => x.CustomerGroups)
                .WithMany(x => x.AdminMessages_Admin)
                .Map(x =>
                {
                    x.ToTable("tbl_AdminMessages_Admin_CustomerGroup");
                    x.MapLeftKey("CustomerGroup_Id");
                    x.MapRightKey("AdminMessages_Admin_Id");
                });

            HasMany(x => x.Customers)
                .WithMany(x => x.AdminMessages_Admin)
                .Map(x =>
                {
                    x.ToTable("tbl_AdminMessages_Admin_Customer");
                    x.MapLeftKey("Customer_Id");
                    x.MapRightKey("AdminMessages_Admin_Id");
                });

            HasMany(x => x.AdminMessages_Customer)
                .WithRequired(x => x.AdminMessage_Admin)
                .WillCascadeOnDelete(false);
        }
    }
}
