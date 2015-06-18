using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_Order_Value_CheckboxGroup")]
    public class Order_Value_CheckboxGroup : Order_Value
    {
        public virtual ICollection<FormField_CheckBoxGroup_Item> Values { get; set; }
    }
}
