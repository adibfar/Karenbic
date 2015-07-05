using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_FormGroup")]
    public class FormGroup
    {
        public FormGroup()
        {
            IsShow = true;
            Column = 1;
            Priority = 0;
        }

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public Portal Portal { get; set; }

        [Required]
        public bool IsShow { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [Range(1, 3)]
        public int Column { get; set; }

        [Required]
        [Range(0, Int32.MaxValue)]
        public int Priority { get; set; }

        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public ICollection<Form> Forms { get; set; }
    }
}
