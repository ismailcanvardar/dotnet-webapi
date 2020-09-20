using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Commander.Models
{
    public class User
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

        [DefaultValue(null)]
        public string ProfilePhoto { get; set; }

        [DefaultValue(0)]
        public int RatingCount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}