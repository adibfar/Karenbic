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
        }

        [Required]
        public decimal Prepayment { get; set; }
    }
}
