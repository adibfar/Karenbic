﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_FormField")]
    public class FormField
    {
        public FormField()
        {
            ShowCustomer = true;
            ShowAdmin = true;
            CanDelete = true;
            Priority = 0;
        }

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public bool ShowCustomer { get; set; }

        [Required]
        public bool ShowAdmin { get; set; }

        public string Description { get; set; }

        public string PictureHelpFile { get; set; }

        [NotMapped]
        public string PictureHelpPath
        {
            get
            {
                return string.Format("/Content/FormField/{0}", PictureHelpFile);
            }
        }

        [NotMapped]
        public bool HasPictureHelpFile {
            get
            {
                return !string.IsNullOrEmpty(PictureHelpFile);
            }
        }

        [Required]
        public bool CanDelete { get; set; }

        [Required]
        [Range(0, Int32.MaxValue)]
        public int Priority { get; set; }

        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public virtual Form Form { get; set; }
        public virtual FormField_Position_Desktop DesktopPosition { get; set; }
        public virtual FormField_Position_Tablet TabletPosition { get; set; }
        public virtual FormField_Position_Mobile MobilePosition { get; set; }
        public virtual ICollection<OrderPriceValue> OrderPriceValues { get; set; }
        public virtual ICollection<Order_Value> Orders_Value { get; set; }
    }
}
