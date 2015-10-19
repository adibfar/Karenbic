using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_PrintFactor")]
    public class PrintFactor
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public decimal PrintPrice { get; set; }

        [Required]
        public decimal TransportPrice { get; set; }

        [NotMapped]
        public decimal Price
        {
            get
            {
                return PrintPrice + TransportPrice;
            }
        }

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

        public virtual PrintOrder Order { get; set; }

        public virtual PrintPayment Payment { get; set; }
    }
}
