using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_PortfolioType")]
    public class PortfolioType
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [Range(0, Int32.MaxValue)]
        public int Priority { get; set; }

        [Required]
        public string PictureFile { get; set; }

        [NotMapped]
        public string PicturePath
        {
            get
            {
                return string.Format("/Content/Portfolio/{0}", PictureFile);
            }
        }

        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public virtual ICollection<PortfolioCategory> Categories { get; set; }
    }
}
