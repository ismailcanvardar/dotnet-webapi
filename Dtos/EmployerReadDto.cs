﻿using System.ComponentModel.DataAnnotations;

namespace KariyerAppApi.Models
{
    public class EmployerReadDto
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string ProfilePhoto { get; set; }

        public string Address { get; set; }

        public string Province { get; set; }

        public string District { get; set; }
    }
}