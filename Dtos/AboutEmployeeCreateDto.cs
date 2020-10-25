using System;
using System.ComponentModel.DataAnnotations;

namespace KariyerAppApi.Dtos
{
    public class AboutEmployeeCreateDto
    {
        [Required]
        public Guid EmployeeId { get; set; }
    }
}
