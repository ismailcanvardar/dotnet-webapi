using System;
using System.ComponentModel.DataAnnotations;

namespace Commander.Dtos
{
    public class PickedEmployeeCreateDto
    {
        [Required]
        public string EmployeeExternalId { get; set; }

        [Required]
        public string AdvertExternalId { get; set; }
    }
}
