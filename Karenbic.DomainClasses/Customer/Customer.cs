using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karenbic.DomainClasses
{
    [Table("tbl_Customer")]
    public class Customer
    {
        public Customer()
        {
            RegisterDate = DateTime.Now;
        }

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Index(IsUnique = true)]
        [StringLength(450)]
        [Required]
        public string Username { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public DateTime RegisterDate { get; set; }

        public string Phone { get; set;}

        [Required]
        public string Mobile { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        /*=-=-=-=-=-=-= Relations =-=-=-=-=-=-=*/

        public virtual City City { get; set; }

        public virtual CustomerGroup Group { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
