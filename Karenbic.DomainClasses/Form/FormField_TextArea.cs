using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_FormField_TextArea")]
    public class FormField_TextArea : FormField
    {
        [Required]
        public bool IsRequired { get; set; }

        [Required]
        public bool CharacterLimits { get; set; }

        [Required]
        public int MinCharacters { get; set; }

        [Required]
        public int MaxCharacters { get; set; }

        [Required]
        public bool ShowInFactor { get; set; }

        [Required]
        public int FactorOrder { get; set; }

        [Required]
        public int Height { get; set; }
    }
}
