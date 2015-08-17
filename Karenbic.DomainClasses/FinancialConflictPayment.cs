using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_FinancialConflictPayment")]
    public class FinancialConflictPayment :Payment
    {
        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public virtual FinancialConflict FinancialConflict { get; set; }
    }
}
