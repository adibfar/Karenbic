using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_Message")]
    public class Message
    {
        public Message()
        {
            Id = Guid.NewGuid();
            SendDate = DateTime.Now;
        }

        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public DateTime SendDate { get; set; }
    }
}
