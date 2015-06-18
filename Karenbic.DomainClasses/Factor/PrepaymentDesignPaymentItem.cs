using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_PrepaymentDesignPaymentItem")]
    public class PrepaymentDesignPaymentItem
    {
        [Key]
        [Required]
        public int id { get; set; }

        [Required]
        public int FactorId { get; set; }

        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public virtual DesignPayment Payment { get; set; }
    }
}
