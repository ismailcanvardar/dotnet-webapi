using System;
using System.ComponentModel.DataAnnotations;

namespace Commander.Models
{
    public class PickedEmployee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ExternalId { get; set; }

        [Required]
        public string EmployerExternalId { get; set; }

        [Required]
        public string EmployeeExternalId { get; set; }

        [Required]
        public string AdvertExternalId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
