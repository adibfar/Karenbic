using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_FormField_DropDown")]
    public class FormField_DropDown : FormField
    {
        [Required]
        public bool IsRequired { get; set; }

        [Required]
        public bool ShowInFactor { get; set; }

        [Required]
        public int FactorOrder { get; set; }

        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public virtual ICollection<FormField_DropDown_Item> Items { get; set; }
    }
}
