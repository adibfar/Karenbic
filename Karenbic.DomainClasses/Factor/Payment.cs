using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_Payment")]
    public class Payment
    {
        public Payment()
        {
            IsComplete = false;
        }

        [Key]
        [Required]
        public int Id { get; set; }

        [NotMapped]
        public int Code
        {
            get
            {
                return Id + 1024;
            }
        }

        [Required]
        public decimal Money { get; set; }

        [Required]
        public DateTime RegisterDate { get; set; }

        [NotMapped]
        public string PersianRegisterDate
        {
            get
            {
                return Api.ConvertDate.JulainToPersian(RegisterDate);
            }
        }

        [Required]
        public bool IsComplete { get; set; }

        [Required]
        public bool IsPaid { get; set; }

        /*=-=-=-=-=-=-= Bank Data =-=-=-=-=-=-=*/

        public string RefId { get; set; }

        public string ResCode { get; set; }

        public long? SaleOrderId { get; set; }

        public long? SaleReferenceId { get; set; }        
    }
}
