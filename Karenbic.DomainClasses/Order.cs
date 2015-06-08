using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_Order")]
    public class Order
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime RegisterDate { get; set; }

        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public virtual Form Form { get; set; }

        public virtual ICollection<Order_Value> Values { get; set; }
    }
}
