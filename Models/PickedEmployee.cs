using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KariyerAppApi.Models
{
    public class PickedEmployee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PickedEmployeeId { get; set; }

        [Required]
        public Guid EmployerId { get; set; }

        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        public Guid AdvertId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
