using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_Order_Value_TextArea")]
    public class Order_Value_TextArea : Order_Value
    {
        public string Value { get; set; }
    }
}
