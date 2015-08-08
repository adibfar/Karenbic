using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_OrderPriceValue")]
    public class OrderPriceValue
    {
        public OrderPriceValue()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        [Required]
        public Guid Id { get; set; }

        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public virtual OrderPrice OrderPrice { get; set; }

        public virtual FormField Field { get; set; }
    }
}
