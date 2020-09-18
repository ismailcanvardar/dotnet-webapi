using System.ComponentModel.DataAnnotations;

namespace Commander.Models
{
    public class UserLoginDto
    {
        public string Username { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string ProfilePhoto { get; set; }

        public string Token { get; set; }
    }
}