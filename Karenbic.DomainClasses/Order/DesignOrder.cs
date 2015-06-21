using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_DesignOrder")]
    public class DesignOrder : Order
    {
        public DesignOrder()
        {
            Prepayment = 0;
            IsPaidPrepayment = false;
            IsPaidFinal = false;
            OrderState = DesignOrderState.Register;
        }

        [Required]
        public decimal Prepayment { get; set; }

        [Required]
        public bool IsPaidPrepayment { get; set; }

        [Required]
        public bool IsPaidFinal { get; set; }

        [Required]
        public DesignOrderState OrderState { get; set; }

        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public virtual PrepaymentDesignFactor PrepaymentFactor { get; set; }

        public virtual FinalDesignFactor FinalFactor { get; set; }
    }
}
