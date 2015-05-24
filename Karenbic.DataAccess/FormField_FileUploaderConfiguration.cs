using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class FormField_FileUploaderConfiguration : EntityTypeConfiguration<DomainClasses.FormField_FileUploader>
    {
        public FormField_FileUploaderConfiguration()
        {
            HasMany(x => x.Formats)
                .WithMany(x => x.FileUploaders)
                .Map(x => 
                {
                    x.ToTable("tbl_FormField_FileUploader_FileFormat");
                    x.MapLeftKey("FileFormat_Id");
                    x.MapRightKey("FormField_FileUploader_Id");
                });
        }
    }
}
