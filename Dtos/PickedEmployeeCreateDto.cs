using System;
using System.ComponentModel.DataAnnotations;

namespace Commander.Dtos
{
    public class PickedEmployeeCreateDto
    {
        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        public Guid AdvertId { get; set; }
    }
}
