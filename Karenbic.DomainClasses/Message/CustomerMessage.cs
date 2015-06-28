using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_CustomerMessage")]
    public class CustomerMessage : Message
    {
        public CustomerMessage()
        {
            IsShowAdmin = true;
            IsShowCustomer = true;

            IsReadAdmin = false;
            IsReadCustomer = true;

            IsAdminReply = false;
        }

        [Required]
        public bool IsShowAdmin { get; set; }

        [Required]
        public bool IsShowCustomer { get; set; }

        [Required]
        public bool IsReadAdmin { get; set; }

        [Required]
        public bool IsReadCustomer { get; set; }

        [Required]
        public bool IsAdminReply { get; set; }

        public string AdminReply { get; set; }

        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public Customer Sender { get; set; }
    }
}
