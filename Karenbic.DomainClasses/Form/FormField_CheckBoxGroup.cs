using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_FormField_CheckBoxGroup")]
    public class FormField_CheckBoxGroup : FormField
    {
        [Required]
        public bool ShowInFactor { get; set; }

        [Required]
        public int FactorOrder { get; set; }

        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public virtual ICollection<FormField_CheckBoxGroup_Item> Items { get; set; }
    }
}
