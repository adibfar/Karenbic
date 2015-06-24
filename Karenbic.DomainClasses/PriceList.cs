using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_PriceList")]
    public class PriceList
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public Portal Portal { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int Order { get; set; }

        public string PictureFile { get; set; }

        [NotMapped]
        public string PicturePath
        {
            get
            {
                return string.Format("/Content/PriceList/{0}", PictureFile);
            }
        }
    }
}
