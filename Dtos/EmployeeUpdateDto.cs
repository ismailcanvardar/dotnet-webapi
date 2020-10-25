using System;
namespace KariyerAppApi.Dtos
{
    public class EmployeeUpdateDto
    {
        public Guid EmployeeId { get; set; }

        public string Phone { get; set; }

        public string ProfilePhoto { get; set; }

        public string Address { get; set; }
    }
}
