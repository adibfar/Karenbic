using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Karenbic.DomainClasses
{
    [Table("tbl_PrintOrder")]
    public class PrintOrder : Order
    {
        public PrintOrder()
        {
            IsPaid = false;
        }

        [Required]
        public bool IsPaid { get; set; }

        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public virtual PrintFactor Factor { get; set; }
    }
}
