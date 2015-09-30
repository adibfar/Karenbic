using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_HomeSlideShow")]
    public class HomeSlideShow
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [Range(0, Int32.MaxValue)]
        public int Priority { get; set; }

        [Required]
        public string PictureFile { get; set; }

        public string PicturePath
        {
            get
            {
                return string.Format("/Content/HomeSlideShow/{0}", PictureFile);
            }
        }
    }
}
