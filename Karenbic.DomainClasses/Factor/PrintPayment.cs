using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_PrintPayment")]
    public class PrintPayment : Payment
    {
        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public virtual ICollection<PrintPaymentItem> Items { get; set; }

        public virtual ICollection<PrintFactor> Factors { get; set; }
    }
}
