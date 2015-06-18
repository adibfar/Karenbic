using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_FinalDesignFactor")]
    public class FinalDesignFactor
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public decimal Price { get; set; }

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

        /*=-=-=-=-=-=-= Pay Data =-=-=-=-=-=-=*/

        [Required]
        public bool IsPaid { get; set; }

        public DateTime? PaidDate { get; set; }

        [NotMapped]
        public string PersianPaidDate
        {
            get
            {
                return Api.ConvertDate.JulainToPersian(Convert.ToDateTime(PaidDate));
            }
        }

        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public virtual DesignOrder Order { get; set; }

        public virtual DesignPayment Payment { get; set; }
    }
}
