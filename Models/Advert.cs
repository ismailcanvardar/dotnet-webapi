using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KariyerAppApi.Models
{
    public class Advert
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AdvertId { get; set; }

        [Required]
        public Guid EmployerId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public float DailySalary { get; set; }

        [Required]
        public int NeededEmployee { get; set; }

        [Required]
        public string Province { get; set; }

        [Required]
        public string District { get; set; }

        [Required]
        public string Neighborhood { get; set; }

        [DefaultValue(null)]
        public string Thumbnail { get; set; }

        [DefaultValue(0)]
        public int TotalApplicantCount { get; set; }

        [DefaultValue(true)]
        public bool IsVisible { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
