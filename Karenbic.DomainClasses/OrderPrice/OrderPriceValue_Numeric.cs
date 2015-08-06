using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_OrderPriceValue_Numeric")]
    public class OrderPriceValue_Numeric : OrderPriceValue
    {
        [Required]
        public float MinValue { get; set; }

        [Required]
        public float MaxValue { get; set; }
    }
}
