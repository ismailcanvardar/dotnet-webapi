using System.ComponentModel.DataAnnotations;

namespace Commander.Models
{
    public class EmployeeLoginDto
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string ProfilePhoto { get; set; }

        public string Token { get; set; }
    }
}