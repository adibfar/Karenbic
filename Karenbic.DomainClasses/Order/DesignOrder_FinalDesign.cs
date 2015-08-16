using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_DesignOrder_FinalDesign")]
    public class DesignOrder_FinalDesign
    {
        public int Id { get; set; }

        public string Title { get; set; }

        [Required]
        public string Link { get; set; }

        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public virtual DesignOrder Order { get; set; }
    }
}
