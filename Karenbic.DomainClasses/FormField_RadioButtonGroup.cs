using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_FormField_RadioButtonGroup")]
    public class FormField_RadioButtonGroup : FormField
    {
        [Required]
        public bool IsRequired { get; set; }

        [Required]
        public bool ShowInFactor { get; set; }

        [Required]
        public int FactorOrder { get; set; }

        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public virtual ICollection<FormField_RadioButtonGroup_Item> Items { get; set; }
    }
}
