using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KariyerAppApi.Models
{
    public class AboutEmployee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AboutEmployeeId { get; set; }

        [Required]
        public Guid EmployeeId { get; set; }

        [DefaultValue("")]
        public string Job { get; set; }

        [DefaultValue("")]
        public string BriefInformation { get; set; } 

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
