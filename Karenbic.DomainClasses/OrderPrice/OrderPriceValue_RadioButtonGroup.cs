using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_OrderPriceValue_RadioButtonGroup")]
    public class OrderPriceValue_RadioButtonGroup : OrderPriceValue
    {
        public virtual FormField_RadioButtonGroup_Item Value { get; set; }
    }
}
