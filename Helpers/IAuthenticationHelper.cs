using System;
using KariyerAppApi.Models;
using Microsoft.AspNetCore.Http;

namespace KariyerAppApi.Helpers
{
    public interface IAuthenticationHelper
    {
        bool IsEmployer();
        bool IsUser();
        Guid GetCurrentUserId();
        string GenerateJSONWebTokenForEmployee(Employee employee);
        string GenerateJSONWebTokenForEmployer(Employer employer);
    }
}
