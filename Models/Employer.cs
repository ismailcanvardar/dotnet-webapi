using System;
using System.ComponentModel.DataAnnotations;

namespace Commander.Models
{
    public class Employer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ExternalId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Address { get; set; }

        public string ProfilePhoto { get; set; }
    }
}
