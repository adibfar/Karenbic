using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_FormField_DatePicker")]
    public class FormField_DatePicker : FormField
    {
        [Required]
        public bool IsRequired { get; set; }

        [Required]
        public bool Limits { get; set; }

        [Required]
        public float Min { get; set; }

        [Required]
        public float Max { get; set; }

        [Required]
        public bool ShowInFactor { get; set; }

        [Required]
        public int FactorOrder { get; set; }
    }
}
