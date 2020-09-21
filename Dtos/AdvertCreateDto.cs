using System;
using System.ComponentModel.DataAnnotations;

namespace Commander.Dtos
{
    public class AdvertCreateDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public float DailySalary { get; set; }

        [Required]
        public int NeededEmployee { get; set; }

        [Required]
        public string Province { get; set; }

        [Required]
        public string District { get; set; }

        [Required]
        public string Neighborhood { get; set; }
    }
}
