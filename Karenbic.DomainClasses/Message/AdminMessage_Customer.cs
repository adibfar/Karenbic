using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_AdminMessage_Customer")]
    public class AdminMessage_Customer : Message
    {
        public AdminMessage_Customer()
        {
            IsRead = false;
        }

        [Required]
        public bool IsRead { get; set; }

        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public virtual Customer Customer { get; set; }

        public virtual AdminMessage_Admin AdminMessage_Admin { get; set; }
    }
}
