using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_Order_Value")]
    public class Order_Value
    {
        public Order_Value()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        [Required]
        public Guid Id { get; set; }

        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public virtual Order Order { get; set; }

        public virtual FormField Field { get; set; }
    }
}
