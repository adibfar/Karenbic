using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_PrintPaymentItem")]
    public class PrintPaymentItem
    {
        [Key]
        [Required]
        public int id { get; set; }

        [Required]
        public int FactorId { get; set; }

        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public virtual PrintPayment Payment { get; set; }
    }
}
