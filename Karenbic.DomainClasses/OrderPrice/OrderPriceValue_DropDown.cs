using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_OrderPriceValue_DropDown")]
    public class OrderPriceValue_DropDown : OrderPriceValue
    {
        public virtual FormField_DropDown_Item Value { get; set; }
    }
}
