using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_Order_Value_DatePicker")]
    public class Order_Value_DatePicker : Order_Value
    {
        public DateTime? Value { get; set; }
    }
}
