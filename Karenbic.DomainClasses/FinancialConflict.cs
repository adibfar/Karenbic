using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_FinancialConflict")]
    public class FinancialConflict
    {
        public FinancialConflict()
        {
            IsPaid = false;
        }

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public Portal Portal { get; set; }

        [Required]
        public string Description { get; set; }

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

        [Required]
        public bool IsPaid { get; set; }

        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public virtual Customer Customer { get; set; }

        public virtual ICollection<FinancialConflictItem> Items { get; set; }
    }
}
