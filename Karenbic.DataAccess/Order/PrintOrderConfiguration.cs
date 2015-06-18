using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class PrintOrderConfiguration : EntityTypeConfiguration<DomainClasses.PrintOrder>
    {
        public PrintOrderConfiguration()
        {
            HasOptional(x => x.Factor)
                .WithRequired(x => x.Order)
                .WillCascadeOnDelete(true);
        }
    }
}
