using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_FormField_RadioButtonGroup_Item")]
    public class FormField_RadioButtonGroup_Item
    {
        public FormField_RadioButtonGroup_Item()
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
        public int Order { get; set; }

        [Required]
        public bool ShowCustomer { get; set; }

        [Required]
        public bool ShowAdmin { get; set; }

        [Required]
        public bool CanDelete { get; set; }

        public string PictureHelpFile { get; set; }

        [NotMapped]
        public string PictureHelpPath
        {
            get
            {
                return string.Format("/Content/FormField/{0}", PictureHelpFile);
            }
        }

        [NotMapped]
        public bool HasPictureHelpFile
        {
            get
            {
                return !string.IsNullOrEmpty(PictureHelpFile);
            }
        }

        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public virtual FormField_RadioButtonGroup RadioButtonGroup { get; set; }
        public virtual ICollection<OrderPriceValue_RadioButtonGroup> OrderPriceValues { get; set; }
        public virtual ICollection<Order_Value_RadioButtonGroup> Orders_Value { get; set; }
    }
}
