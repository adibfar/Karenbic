using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_Order_Value_FileUploader2")]
    public class Order_Value_FileUploader2 : Order_Value
    {
        //1: File
        //2: Design
        [Required]
        public int Type { get; set; }

        public string FileName { get; set; }

        [NotMapped]
        public string FilePath
        {
            get
            {
                return string.Format("/Content/Order/{0}", FileName);
            }
        }

        [NotMapped]
        public bool HasFile
        {
            get
            {
                return !string.IsNullOrEmpty(FileName);
            }
        }

        public int? DesignOrderId { get; set; }
    }
}
