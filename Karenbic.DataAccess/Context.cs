using System;
using System.Data.Entity;

namespace Karenbic.DataAccess
{
    public class Context : DbContext
    {
        public Context()
            : base("cn1")
        {

        }

        public DbSet<DomainClasses.FileFormat> FileFormats { get; set; }

        public DbSet<DomainClasses.Form> Forms { get; set; }
        public DbSet<DomainClasses.FormField> FormFields { get; set; }
        public DbSet<DomainClasses.FormField_TextBox> FormFields_TextBox { get; set; }
        public DbSet<DomainClasses.FormField_TextArea> FormFields_TextArea { get; set; }
        public DbSet<DomainClasses.FormField_Numeric> FormFileds_Numeric { get; set; }
        public DbSet<DomainClasses.FormField_ColorPicker> FormFields_ColorPicker { get; set; }
        public DbSet<DomainClasses.FormField_FileUploader> FormFields_FileUploader { get; set; }
        public DbSet<DomainClasses.FormField_DropDown> FormFields_DropDown { get; set; }
        public DbSet<DomainClasses.FormField_DropDown_Item> FormField_DropDown_Items { get; set; }
        public DbSet<DomainClasses.FormField_RadioButtonGroup> FormFields_RadioButtonGroup { get; set; }
        public DbSet<DomainClasses.FormField_RadioButtonGroup_Item> FormField_RadioButtonGroup_Items { get; set; }
        public DbSet<DomainClasses.FormField_CheckBox> FormFields_CheckBox { get; set; }
        public DbSet<DomainClasses.FormField_CheckBoxGroup> FormFields_CheckBoxGroup { get; set; }
        public DbSet<DomainClasses.FormField_CheckBoxGroup_Item> FormField_CheckBoxGroup_Items { get; set; }
        public DbSet<DomainClasses.FormField_WebUrl> FormFields_WebUrl{ get; set; }
        public DbSet<DomainClasses.FormField_DatePicker> FormFields_DatePicker { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new FormConfiguration());
            modelBuilder.Configurations.Add(new FormFieldConfiguration());
            modelBuilder.Configurations.Add(new FormField_FileUploaderConfiguration());
            modelBuilder.Configurations.Add(new FileFormatConfiguration());
            modelBuilder.Configurations.Add(new FormField_DropDownConfiguration());
            modelBuilder.Configurations.Add(new FormField_DropDown_ItemConfiguration());
            modelBuilder.Configurations.Add(new FormField_RadioButtonGroupConfiguration());
            modelBuilder.Configurations.Add(new FormField_RadioButtonGroup_ItemConfiguration());
            modelBuilder.Configurations.Add(new FormField_CheckBoxGroupConfiguration());
            modelBuilder.Configurations.Add(new FormField_CheckBoxGroup_ItemConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
