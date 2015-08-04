using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_PublicPrice")]
    public class PublicPrice
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int Priority { get; set; }

        public string PictureFile { get; set; }

        [NotMapped]
        public string PicturePath
        {
            get
            {
                return string.Format("/Content/PriceList/{0}", PictureFile);
            }
        }

        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public virtual PublicPriceCategory Category { get; set; }
    }
}
