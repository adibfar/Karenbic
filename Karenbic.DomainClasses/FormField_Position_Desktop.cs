using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_FormField_Position_Desktop")]
    public class FormField_Position_Desktop
    {
        public FormField_Position_Desktop()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public int SizeX { get; set; }

        [Required]
        public int SizeY { get; set; }

        [Required]
        public int Row { get; set; }

        [Required]
        public int Column { get; set; }

        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public virtual FormField FormField { get; set; }
    }
}
