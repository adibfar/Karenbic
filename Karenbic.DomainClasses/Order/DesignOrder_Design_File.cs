using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_DesignOrder_Design_File")]
    public class DesignOrder_Design_File
    {
        public DesignOrder_Design_File()
        {
            Id = Guid.NewGuid();
            State = DesignOrder_Design_File_State.None;
            TempState = DesignOrder_Design_File_State.None;
        }

        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string PictureFile { get; set; }

        [NotMapped]
        public string PicturePath
        {
            get
            {
                return string.Format("/Content/DesignOrder/{0}", PictureFile);
            }
        }

        /*=-=-=-=-=-=-= Result =-=-=-=-=-=-=*/

        [Required]
        public DesignOrder_Design_File_State State { get; set; }

        [Required]
        public DesignOrder_Design_File_State TempState { get; set; }

        public string CustomerDescription { get; set; }

        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public virtual DesignOrder_Design Design { get; set; }
    }
}
