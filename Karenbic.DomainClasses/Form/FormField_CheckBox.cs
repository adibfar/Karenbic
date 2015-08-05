using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_FormField_CheckBox")]
    public class FormField_CheckBox : FormField
    {
        public FormField_CheckBox()
        {
            UseForPrice = false;
        }

        [Required]
        public bool ShowInFactor { get; set; }

        [Required]
        public int FactorOrder { get; set; }

        [Required]
        public bool UseForPrice { get; set; }
    }
}
