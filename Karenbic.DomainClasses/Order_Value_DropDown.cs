using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_Order_Value_DropDown")]
    public class Order_Value_DropDown : Order_Value
    {
        public virtual FormField_DropDown_Item Value { get; set; }
    }
}
