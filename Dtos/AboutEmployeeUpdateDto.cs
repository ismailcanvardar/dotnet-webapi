using System;
using System.ComponentModel.DataAnnotations;

namespace KariyerAppApi.Dtos
{
    public class AboutEmployeeUpdateDto
    {
        [Required]
        public Guid EmployeeId { get; set; }

        public string Job { get; set; }
        
        public string BriefInformation { get; set; }
    }
}
