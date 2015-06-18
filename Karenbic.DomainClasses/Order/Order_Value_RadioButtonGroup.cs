using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_Order_Value_RadioButtonGroup")]
    public class Order_Value_RadioButtonGroup : Order_Value
    {
        public virtual FormField_RadioButtonGroup_Item Value { get; set; }
    }
}
