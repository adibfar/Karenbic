using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_FormField_ColorPicker")]
    public class FormField_ColorPicker : FormField
    {
        [Required]
        public bool IsRequired { get; set; }
    }
}
