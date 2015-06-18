using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_DesignPayment")]
    public class DesignPayment : Payment
    {
        public virtual ICollection<PrepaymentDesignPaymentItem> PrepaymentItems { get; set; }

        public virtual ICollection<FinalDesignPaymentItem> FinalItems { get; set; }

        public virtual ICollection<PrepaymentDesignFactor> PrepaymentFactors { get; set; }

        public virtual ICollection<FinalDesignFactor> FinalFactors { get; set; }
    }
}
