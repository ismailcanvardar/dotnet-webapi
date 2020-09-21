using System;
using System.ComponentModel.DataAnnotations;

namespace Commander.Dtos
{
    public class AdvertReadDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public float DailySalary { get; set; }

        public int NeededEmployee { get; set; }

        public string Province { get; set; }

        public string District { get; set; }

        public string Neighborhood { get; set; }

        public string Thumbnail { get; set; }

        public int TotalParticipantCount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
