﻿using System;
using System.ComponentModel.DataAnnotations;

namespace KariyerAppApi.Dtos
{
    public class PickedEmployeeRemoveDto
    {
        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        public Guid AdvertId { get; set; }
    }
}
