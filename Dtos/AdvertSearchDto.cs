using System;
using System.ComponentModel.DataAnnotations;

namespace KariyerAppApi.Dtos
{
    public class AdvertSearchDto
    {
        public string SearchCriteria { get; set; }

        public string Province { get; set; }

        public string District { get; set; }

        public string Neighborhood { get; set; }
    }
}
