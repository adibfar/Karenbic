using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_DesignOrder_Design")]
    public class DesignOrder_Design
    {
        public DesignOrder_Design()
        {
            RegisterDate = DateTime.Now;
            IsReview = false;
        }

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime RegisterDate { get; set; }

        public string Description { get; set; }

        [Required]
        public bool IsReview { get; set; }

        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public virtual DesignOrder Order { get; set; }

        public virtual ICollection<DesignOrder_Design_File> Files { get; set; }
    }
}
