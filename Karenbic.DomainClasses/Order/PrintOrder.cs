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
            PrintPrice = 0;
            PackingPrice = 0;
            IsPaid = false;
            OrderState = PrintOrderState.Register;
            TransportType = DomainClasses.TransportType.None;
            
        }

        [Required]
        public decimal PrintPrice { get; set; }

        /// <summary>
        /// بسته بندی و حمل نقل
        /// طی تغییرات نام به علت وابستگی های زیاد صروت نگرفت
        /// دیوانه ها :|
        /// </summary>
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

        [Required]
        public bool IsPaid { get; set; }

        [Required]
        public PrintOrderState OrderState { get; set; }

        [Required]
        public TransportType TransportType { get; set; }

        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public virtual PrintFactor Factor { get; set; }
    }
}
