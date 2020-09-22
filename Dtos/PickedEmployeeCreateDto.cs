using System;
using System.ComponentModel.DataAnnotations;

namespace KariyerAppApi.Dtos
{
    public class PickedEmployeeCreateDto
    {
        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        public Guid AdvertId { get; set; }
    }
}
