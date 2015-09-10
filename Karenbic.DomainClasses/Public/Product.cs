using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_Product")]
    public class Product
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public int Priority { get; set; }

        [Required]
        public decimal Price { get; set; }

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

        public virtual ProductCategory Category { get; set; }

        public virtual ICollection<ProductPicture> Pictures { get; set; }
    }
}
