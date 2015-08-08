using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Karenbic.Areas.Customer.Models
{
    public class OrderPriceValue_CheckboxGroup : OrderPriceValue
    {
        public int[] Values { get; set; }
    }
}