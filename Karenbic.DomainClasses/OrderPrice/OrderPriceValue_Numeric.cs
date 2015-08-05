using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_OrderPriceValue_Numeric")]
    public class OrderPriceValue_Numeric : OrderPriceValue
    {
        public float? Value { get; set; }
    }
}
