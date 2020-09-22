using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Commander.Models
{
    public class Rating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid RatingId { get; set; }

        [Required]
        public Guid EmployerId { get; set; }

        [Required]
        public Guid EmployeeId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
