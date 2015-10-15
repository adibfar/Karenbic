using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_FormField_Label")]
    public class FormField_Label : FormField
    {
        public string Color { get; set; }

        public string FontFamily { get; set; }

        [Required]
        public int FontSize { get; set; }

        [Required]
        public bool Underline { get; set; }

        [Required]
        public bool Upline { get; set; }
    }
}
