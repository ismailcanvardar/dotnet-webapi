using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KariyerAppApi.Models
{
    public class Application
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ApplicationId { get; set; }

        [Required]
        public Guid EmployerId { get; set; }

        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        public Guid AdvertId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
