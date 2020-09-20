using System;
using System.ComponentModel.DataAnnotations;

namespace Commander.Dtos
{
    public class AdvertSearchDto
    {
        public string Province { get; set; }

        public string District { get; set; }

        public string Neighborhood { get; set; }
    }
}
