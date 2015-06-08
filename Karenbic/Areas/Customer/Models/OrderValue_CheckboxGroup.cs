using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Karenbic.Areas.Customer.Models
{
    public class OrderValue_CheckboxGroup : OrderValue
    {
        public int[] Values { get; set; }
    }
}