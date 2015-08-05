using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_OrderPriceValue_CheckboxGroup")]
    public class OrderPriceValue_CheckboxGroup : OrderPriceValue
    {
        public virtual ICollection<FormField_CheckBoxGroup_Item> Values { get; set; }
    }
}
