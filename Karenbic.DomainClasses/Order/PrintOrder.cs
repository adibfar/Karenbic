﻿using System;
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
            OrderState = PrintOrderState.Register;
            
        }

        [Required]
        public bool IsPaid { get; set; }

        [Required]
        public PrintOrderState OrderState { get; set; }

        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public virtual PrintFactor Factor { get; set; }
    }
}