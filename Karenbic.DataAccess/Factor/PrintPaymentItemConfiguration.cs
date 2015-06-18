using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class PrintPaymentItemConfiguration : EntityTypeConfiguration<DomainClasses.PrintPaymentItem>
    {
        public PrintPaymentItemConfiguration()
        {
            HasRequired(x => x.Payment)
                .WithMany(x => x.Items)
                .WillCascadeOnDelete(false);
        }
    }
}
