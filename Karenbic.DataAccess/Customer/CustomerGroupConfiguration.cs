using System.Data.Entity.ModelConfiguration;
namespace Karenbic.DataAccess
{
    class CustomerGroupConfiguration : EntityTypeConfiguration<DomainClasses.CustomerGroup>
    {
        public CustomerGroupConfiguration()
        {
            HasMany(x => x.Customers)
                .WithOptional(x => x.Group)
                .WillCascadeOnDelete(false);

            HasMany(x => x.AdminMessages_Admin)
                .WithMany(x => x.CustomerGroups)
                .Map(x =>
                {
                    x.ToTable("tbl_AdminMessages_Admin_CustomerGroup");
                    x.MapLeftKey("AdminMessages_Admin_Id");
                    x.MapRightKey("CustomerGroup_Id");
                });
        }
    }
}
