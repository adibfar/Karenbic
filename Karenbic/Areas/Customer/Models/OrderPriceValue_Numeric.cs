using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Karenbic.Areas.Customer.Models
{
    public class OrderPriceValue_Numeric : OrderPriceValue
    {
        public float MinValue { get; set; }
        public float MaxValue { get; set; }
    }
}