using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_FormField_WebUrl")]
    public class FormField_WebUrl : FormField
    {
        [Required]
        public bool IsRequired { get; set; }
    }
}
