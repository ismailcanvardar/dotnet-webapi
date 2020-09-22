using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KariyerAppApi.Models
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

        [Required]
        public Guid AdvertId { get; set; }

        [Required]
        public int GivenRating { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
