using System;
using System.ComponentModel.DataAnnotations;

namespace KariyerAppApi.Dtos
{
    public class ApplicationCreateDto
    {
        [Required]
        public string EmployerId { get; set; }

        [Required]
        public string AdvertId { get; set; }
    }
}
