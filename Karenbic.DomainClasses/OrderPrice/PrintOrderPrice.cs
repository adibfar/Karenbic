using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_PrintOrderPrice")]
    public class PrintOrderPrice : OrderPrice
    {
        [Required]
        public decimal PrintPrice { get; set; }

        [Required]
        public decimal PackingPrice { get; set; }

        [NotMapped]
        public decimal Price
        {
            get
            {
                return PrintPrice + PackingPrice;
            }
        }
    }
}
