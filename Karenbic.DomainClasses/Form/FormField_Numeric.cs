using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_FormFiled_Numeric")]
    public class FormField_Numeric : FormField
    {
        public FormField_Numeric()
        {
            UseForPrice = false;
        }

        [Required]
        public bool IsInt { get; set; }

        [Required]
        public bool IsFloat { get; set; }

        public float? Defualt { get; set; }

        [Required]
        public bool IsRequired { get; set; }

        [Required]
        public bool Limits { get; set; }

        [Required]
        public float Min{ get; set; }

        [Required]
        public float Max { get; set; }

        [Required]
        public bool ShowInFactor { get; set; }

        [Required]
        public int FactorOrder { get; set; }

        [Required]
        public bool UseForPrice { get; set; }
    }
}
