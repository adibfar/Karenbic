using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_OrderPrice")]
    public class OrderPrice
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Priority { get; set; }

        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public virtual Form Form { get; set; }

        public ICollection<OrderPriceValue> Values { get; set; }
    }
}
