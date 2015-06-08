using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_FormField_DropDown_Item")]
    public class FormField_DropDown_Item
    {
        public FormField_DropDown_Item()
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

        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public virtual FormField_DropDown DropDown { get; set; }

        public virtual ICollection<Order_Value_DropDown> Orders_Value { get; set; }
    }
}
