using System.ComponentModel.DataAnnotations;

namespace Commander.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string ExternalId { get; set; }

        [Required]
        public string Username { get; set; }

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

        public string ProfilePhoto { get; set; }
    }
}