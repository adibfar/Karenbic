using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class FormField_FileUploader2Configuration : EntityTypeConfiguration<DomainClasses.FormField_FileUploader2>
    {
        public FormField_FileUploader2Configuration()
        {
            HasMany(x => x.Formats)
                .WithMany(x => x.FileUploader2s)
                .Map(x =>
                {
                    x.ToTable("tbl_FormField_FileUploader2_FileFormat");
                    x.MapLeftKey("FileFormat_Id");
                    x.MapRightKey("FormField_FileUploader2_Id");
                });
        }
    }
}
