using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_FormField")]
    public class FormField
    {
        public FormField()
        {
            ShowCustomer = true;
            ShowAdmin = true;
            CanDelete = true;
        }

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public bool ShowCustomer { get; set; }

        [Required]
        public bool ShowAdmin { get; set; }

        public string Description { get; set; }

        public string PictureHelpFile { get; set; }

        [Required]
        public bool CanDelete { get; set; }

        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public virtual Form Form { get; set; }
        public virtual FormField_Position_Desktop DesktopPosition { get; set; }
        public virtual FormField_Position_Tablet TabletPosition { get; set; }
        public virtual FormField_Position_Mobile MobilePosition { get; set; }
    }
}
