using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_Form")]
    public class Form
    {
        public Form()
        {
            CanDelete = true;
        }

        [Key]
        [Required]
        public int Id{ get; set; }

        [Required]
        public string Title{ get; set; }

        [Required]
        public bool SpecialCreativity { get; set; }

        [Required]
        public bool IsShow { get; set; }

        public string Description { get; set; }

        [Required]
        public bool CanDelete { get; set; }

        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public virtual ICollection<FormField> Fields { get; set; }
    }
}
