using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_Order_Value_FileUploader")]
    public class Order_Value_FileUploader : Order_Value
    {
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
    }
}
