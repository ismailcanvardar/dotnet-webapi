using System;
using System.ComponentModel.DataAnnotations;

namespace Commander.Dtos
{
    public class PickedEmployeeRemoveDto
    {
        [Required]
        public string EmployeeExternalId { get; set; }

        [Required]
        public string AdvertExternalId { get; set; }
    }
}
