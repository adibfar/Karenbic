﻿using System;
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
            LastChange = DateTime.Now;
            Price = 0;
            SpecialCreativity = false;
            Prepayment = 0;
            IsPaidPrepayment = false;
            IsPaidFinal = false;
            IsPreAcceptDesign = false;
            IsAcceptDesign = false;
            IsSendFinalDesign = false;
        }

        [Required]
        public DateTime LastChange { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public bool SpecialCreativity { get; set; }

        [Required]
        public decimal Prepayment { get; set; }

        [Required]
        public bool IsPaidPrepayment { get; set; }

        [Required]
        public bool IsPaidFinal { get; set; }

        [Required]
        public bool IsPreAcceptDesign { get; set; }

        [Required]
        public bool IsAcceptDesign { get; set; }

        [Required]
        public bool IsSendFinalDesign { get; set; }

        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public virtual PrepaymentDesignFactor PrepaymentFactor { get; set; }

        public virtual FinalDesignFactor FinalFactor { get; set; }

        public virtual ICollection<DesignOrder_Design> Designs { get; set; }

        public virtual ICollection<DesignOrder_FinalDesign> FinalDesigns { get; set; }
    }
}
