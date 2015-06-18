using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_Order_Value_Checkbox")]
    public class Order_Value_Checkbox : Order_Value
    {
        [Required]
        public bool Value { get; set; }
    }
}
