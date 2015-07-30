using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_Setting")]
    public class Setting
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public string PreDesignOrderText { get; set; }

        public string PrePrintOrderText { get; set; }

        public string AboutUsText { get; set; }

        public string ContactUsText { get; set; }

        public string PublicHelpText { get; set; }
    }
}
