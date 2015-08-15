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
            IsShowAdmin = true;
            CanDelete = true;
        }

        [Key]
        [Required]
        public int Id{ get; set; }

        public Portal Portal { get; set; }

        [Required]
        public string Title{ get; set; }

        [Required]
        public int Priority { get; set; }

        [Required]
        public bool SpecialCreativity { get; set; }

        [Required]
        public bool IsShow { get; set; }

        [Required]
        public bool IsShowAdmin { get; set; }

        public string Description { get; set; }

        [Required]
        public bool CanDelete { get; set; }

        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public virtual FormGroup Group { get; set; }

        public virtual ICollection<FormField> Fields { get; set; }

        public virtual ICollection<OrderPrice> OrderPrices { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
