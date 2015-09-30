using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Karenbic.Areas.Customer.Models
{
    public class OrderValue_FileUploader2 : OrderValue
    {
        public int Type { get; set; }
        public string Value { get; set; }
    }
}