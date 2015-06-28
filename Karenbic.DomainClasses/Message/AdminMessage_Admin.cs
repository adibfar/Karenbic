using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_AdminMessage_Admin")]
    public class AdminMessage_Admin : Message
    {
        public AdminMessage_Admin()
        {
            IsShowAdmin = true;
        }

        [Required]
        public bool IsCustomerGroupFilter { get; set; }

        [Required]
        public bool IsCustomerFilter { get; set; }

        [Required]
        public bool IsShowAdmin { get; set; }

        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public virtual ICollection<DomainClasses.CustomerGroup> CustomerGroups { get; set; }

        public virtual ICollection<DomainClasses.Customer> Customers { get; set; }

        public virtual ICollection<DomainClasses.AdminMessage_Customer> AdminMessages_Customer { get; set; }
    }
}
