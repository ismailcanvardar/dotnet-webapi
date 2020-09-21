using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Commander.Data;
using Commander.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Commander.Controllers
{
    [Route("api/employers")]
    [ApiController]
    public class EmployersController : ControllerBase
    {
        private readonly IEmployerRepo _repository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public EmployersController(IEmployerRepo repository, IMapper mapper, IConfiguration config)
        {
            _repository = repository;
            _mapper = mapper;
            _config = config;
        }

        [HttpGet("login/{email}/{password}")]
        public ActionResult<EmployerLoginDto> LoginEmployer(string email, string password)
        {
            Employer employer = _repository.GetEmployerByEmail(email);

            // Controls if there is a user with the given email or hashed password is true
            // against given password from parameters
            if (employer == null || !BCrypt.Net.BCrypt.Verify(password, employer.Password))
            {
                return Problem(detail: "Username or password incorrect", statusCode: 500, title: "Wrong Credentials");
            }

            var employerLoginDto = _mapper.Map<EmployerLoginDto>(employer);

            employerLoginDto.Token = GenerateJSONWebTokenForEmployer(employer);

            return Ok(employerLoginDto);
        }

        [HttpGet("findEmployer/{externalId}")]
        public ActionResult GetEmployer(string externalId)
        {
            var foundEmployer = _repository.GetEmployer(externalId);

            if (foundEmployer == null)
            {
                return NotFound();
            }

            return Ok(foundEmployer);
        }

        [HttpGet("findEmployers/{searchCriteria}/{offset}/{limit}")]
        public ActionResult<IEnumerable<EmployerReadDto>> SearchEmployers(string searchCriteria, int offset, int limit)
        {
            var employers = _repository.SearchEmployers(searchCriteria, offset, limit);

            return Ok(_mapper.Map<IEnumerable<EmployerReadDto>>(employers));
        }

        [HttpPost]
        public ActionResult RegisterEmployer([FromBody] EmployerCreateDto employerCreateDto)
        {
            var employerModel = _mapper.Map<Employer>(employerCreateDto);

            employerModel.Password = BCrypt.Net.BCrypt.HashPassword(employerModel.Password);

            _repository.RegisterEmployer(employerModel);
            _repository.SaveChanges();

            var employerReadDto = _mapper.Map<EmployerReadDto>(employerModel);

            return Ok(employerReadDto);
        }

        private string GenerateJSONWebTokenForEmployer(Employer employer)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("Role", "Employer"),
                new Claim("ExternalId", employer.ExternalId)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
