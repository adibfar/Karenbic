using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_Order")]
    public class Order
    {
        public Order()
        {
            IsCanceled = false;
            IsConfirm = false;
            Price = 0;
        }

        [Key]
        [Required]
        public int Id { get; set; }

        [NotMapped]
        public string Code
        {
            get
            {
                return (Id + 1024).ToString();
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

        /*=-=-=-=-=-= Confirmed Data =-=-=-=-=-=*/

        [Required]
        public bool IsConfirm { get; set; }

        public DateTime? ConfirmDate { get; set; }

        [NotMapped]
        public string PersianConfirmDate
        {
            get
            {
                if (ConfirmDate != null)
                    return Api.ConvertDate.JulainToPersian(Convert.ToDateTime(ConfirmDate));
                else
                    return string.Empty;
            }
        }

        [Required]
        public decimal Price { get; set; }

        /*=-=-=-=-=-= Canceled Data =-=-=-=-=-=*/

        [Required]
        public bool IsCanceled { get; set; }

        public DateTime? CancelDate { get; set; }

        [NotMapped]
        public string PersianCancelDate
        {
            get
            {
                if (CancelDate != null)
                    return Api.ConvertDate.JulainToPersian(Convert.ToDateTime(CancelDate));
                else
                    return string.Empty;
            }
        }

        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public virtual Customer Customer { get; set; }

        public virtual Form Form { get; set; }

        public virtual ICollection<Order_Value> Values { get; set; }
    }
}
