using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KariyerAppApi.Models
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid EmployeeId { get; set; }

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

        [Required]
        public string Province { get; set; }

        [Required]
        public string District { get; set; }

        [DefaultValue(null)]
        public string ProfilePhoto { get; set; }

        [DefaultValue(0.0)]
        public float RatingCount { get; set; }

        [DefaultValue(0)]
        public float TotalRating { get; set; }

        [Required]
        public bool CampaignAllowance { get; set; }

        [Required]
        public bool KvkkAgreement { get; set; }

        [Required]
        public bool UserAgreement { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}