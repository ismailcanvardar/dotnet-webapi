using System;
using KariyerAppApi.Models;
using Microsoft.AspNetCore.Http;

namespace KariyerAppApi.Helpers
{
    public interface IAuthenticationHelper
    {
        bool IsEmployer();
        bool IsEmployee();
        Guid GetCurrentUserId();
        string GenerateJSONWebTokenForEmployee(Employee employee);
        string GenerateJSONWebTokenForEmployer(Employer employer);
    }
}
