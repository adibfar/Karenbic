using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_FormField_FileUploader")]
    public class FormField_FileUploader : FormField
    {
        [Required]
        public bool IsRequired { get; set; }

        [Required]
        public bool SizeLimits { get; set; }

        [Required]
        public int MinSize { get; set; }

        [Required]
        public int MaxSize { get; set; }

        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public virtual ICollection<FileFormat> Formats { get; set; }
    }
}
