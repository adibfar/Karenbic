using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_Portfolio")]
    public class Portfolio
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [Range(0, Int32.MaxValue)]
        public int Priority { get; set; }

        [Required]
        public string TumbPictureFile { get; set; }

        [NotMapped]
        public string TumbPicturePath
        {
            get
            {
                return string.Format("/Content/Portfolio/{0}", TumbPictureFile);
            }
        }

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

        public virtual PortfolioCategory Category { get; set; }
    }
}
