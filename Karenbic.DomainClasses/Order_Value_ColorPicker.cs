using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_Order_Value_ColorPicker")]
    public class Order_Value_ColorPicker : Order_Value
    {
        public string Value { get; set; }
    }
}
