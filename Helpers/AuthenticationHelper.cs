using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using KariyerAppApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace KariyerAppApi.Helpers
{
    public class AuthenticationHelper : IAuthenticationHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;

        public AuthenticationHelper(IHttpContextAccessor httpContextAccessor, IConfiguration config)
        {
            _httpContextAccessor = httpContextAccessor;
            _config = config;
        }

        public bool IsEmployer()
        {
            var currentUser = _httpContextAccessor.HttpContext.User;
            var userRole = currentUser.Claims.FirstOrDefault(c => c.Type == "Role").Value;

            if (userRole == "Employer")
            {
                return true;
            }

            return false;
        }

        public bool IsEmployee()
        {
            var currentUser = _httpContextAccessor.HttpContext.User;
            var userRole = currentUser.Claims.FirstOrDefault(c => c.Type == "Role").Value;

            if (userRole == "User")
            {
                return true;
            }

            return false;
        }

        public Guid GetCurrentUserId()
        {
            var currentUser = _httpContextAccessor.HttpContext.User;
            var currentUserId = currentUser.Claims.FirstOrDefault(c => c.Type == "CurrentUserId").Value;

            return Guid.Parse(currentUserId);
        }

        public string GenerateJSONWebTokenForEmployee(Employee employee)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("Role", "User"),
                new Claim("CurrentUserId", employee.EmployeeId.ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddYears(1),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateJSONWebTokenForEmployer(Employer employer)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("Role", "Employer"),
                new Claim("CurrentUserId", employer.EmployerId.ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddYears(1),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
