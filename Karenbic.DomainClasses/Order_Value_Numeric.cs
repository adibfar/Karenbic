using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_Order_Value_Numeric")]
    public class Order_Value_Numeric : Order_Value
    {
        public float? Value { get; set; }
    }
}
