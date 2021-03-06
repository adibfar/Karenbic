﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_Setting")]
    public class Setting
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public string PreDesignOrderText { get; set; }

        public string PrePrintOrderText { get; set; }

        public string PrintFactorText { get; set; }

        public string DesignFactorText { get; set; }

        public string AboutUsText { get; set; }

        public string ContactUsText { get; set; }

        public string PublicHelpText { get; set; }

        public string AdminMobile { get; set; }

        /// <summary>
        /// هزینه پیک موتوری
        /// </summary>
        public decimal TransportPrice_BikeDelivery { get; set; }

        /// <summary>
        /// هزینه تیپاکس
        /// </summary>
        public decimal TransportPrice_Tipax { get; set; }

        /// <summary>
        /// هزینه تا بابری
        /// </summary>
        public decimal TransportPrice_Porterage { get; set; }
    }
}
