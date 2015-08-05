using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_OrderPriceValue_Checkbox")]
    public class OrderPriceValue_Checkbox : OrderPriceValue
    {
        [Required]
        public bool Value { get; set; }
    }
}
