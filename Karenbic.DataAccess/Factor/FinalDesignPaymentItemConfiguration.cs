using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class FinalDesignPaymentItemConfiguration : EntityTypeConfiguration<DomainClasses.FinalDesignPaymentItem>
    {
        public FinalDesignPaymentItemConfiguration()
        {
            HasRequired(x => x.Payment)
                .WithMany(x => x.FinalItems)
                .WillCascadeOnDelete(false);
        }
    }
}
