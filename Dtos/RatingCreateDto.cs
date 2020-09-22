using System;
using System.ComponentModel.DataAnnotations;

namespace KariyerAppApi.Dtos
{
    public class RatingCreateDto
    {
        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        public Guid AdvertId { get; set; }

        [Required]
        public int GivenRating { get; set; }
    }
}