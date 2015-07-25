using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_PortfolioCategory")]
    public class PortfolioCategory
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [Range(0, Int32.MaxValue)]
        public int Priority { get; set; }

        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public virtual PortfolioType Type { get; set; }

        public virtual ICollection<Portfolio> Portfolios { get; set; }
    }
}
