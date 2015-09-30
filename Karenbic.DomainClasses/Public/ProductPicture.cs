using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_ProductPicture")]
    public class ProductPicture
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string PictureFile { get; set; }

        [NotMapped]
        public string PicturePath
        {
            get
            {
                return string.Format("/Content/Product/{0}", PictureFile);
            }
        }

        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public virtual Product Product { get; set; }
    }
}
