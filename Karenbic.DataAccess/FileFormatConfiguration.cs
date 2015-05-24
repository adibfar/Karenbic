using System.Data.Entity.ModelConfiguration;

namespace Karenbic.DataAccess
{
    class FileFormatConfiguration : EntityTypeConfiguration<DomainClasses.FileFormat>
    {
        public FileFormatConfiguration()
        {
            HasMany(x => x.FileUploaders)
                .WithMany(x => x.Formats)
                .Map(x =>
                {
                    x.ToTable("tbl_FormField_FileUploader_FileFormat");
                    x.MapLeftKey("FormField_FileUploader_Id");
                    x.MapRightKey("FileFormat_Id");
                });
        }
    }
}
