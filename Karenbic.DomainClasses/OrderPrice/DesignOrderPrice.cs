using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_DesignOrderPrice")]
    public class DesignOrderPrice : OrderPrice
    {
        [Required]
        public decimal Price { get; set; }

        [Required]
        public decimal Prepayment { get; set; }

        [Required]
        public decimal SpecialCreativityPrice { get; set; }

        [Required]
        public decimal SpecialCreativityPrepayment { get; set; }
    }
}
