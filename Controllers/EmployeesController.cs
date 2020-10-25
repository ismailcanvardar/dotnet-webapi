using System.Collections.Generic;
using AutoMapper;
using KariyerAppApi.Data;
using KariyerAppApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System;
using Microsoft.Extensions.Configuration;
using KariyerAppApi.Helpers;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using KariyerAppApi.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace KariyerAppApi.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepo _employeeRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IAuthenticationHelper _authenticationHelper;
        private readonly IEmailManagement _emailManagement;
        private readonly IAboutEmployeeRepo _aboutEmployeeRepository;
        private readonly BaseContext _context;

        public EmployeesController(IEmployeeRepo employeeRepository, IMapper mapper, IConfiguration config, IAuthenticationHelper authenticationHelper, IEmailManagement emailManagement, IAboutEmployeeRepo aboutEmployeeRepository, BaseContext context)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _config = config;
            _authenticationHelper = authenticationHelper;
            _emailManagement = emailManagement;
            _aboutEmployeeRepository = aboutEmployeeRepository;
            _context = context;
        }

        [Authorize]
        [HttpGet("getMyProfile")]
        public ActionResult<IQueryable<Employee>> GetMyProfile()
        {
            Guid employeeId = _authenticationHelper.GetCurrentUserId();

            return Ok(_employeeRepository.GetEmployee(employeeId));
        }

        [HttpGet("login/{email}/{password}")]
        public ActionResult<EmployeeLoginDto> LoginEmployee(string email, string password)
        {
            Employee employee = _employeeRepository.GetEmployeeByEmail(email);

            // Controls if there is a user with the given email or hashed password is true
            // against given password from parameters
            if (employee == null || !BCrypt.Net.BCrypt.Verify(password, employee.Password))
            {
                return Problem(detail: "Username or password incorrect", statusCode: 500, title: "Wrong Credentials");
            }

            var employeeLoginDto = _mapper.Map<EmployeeLoginDto>(employee);

            employeeLoginDto.Token = _authenticationHelper.GenerateJSONWebTokenForEmployee(employee);

            return Ok(employeeLoginDto);
        }

        [HttpGet("findUser/{employeeId}")]
        public ActionResult GetEmployee(Guid employeeId)
        {
            var employee = _employeeRepository.GetEmployee(employeeId);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpGet("findUsers/{searchCriteria}/{offset}/{limit}")]
        public ActionResult<IEnumerable<Employee>> SearchUser(string searchCriteria, int offset, int limit)
        {
            var employees = _employeeRepository.SearchEmployees(searchCriteria, offset, limit);

            return Ok(employees);
        }

        [HttpPost]
        public async Task<ActionResult> RegisterUser([FromBody] EmployeeCreateDto employeeCreateDto)
        {
            var foundEmployee = _employeeRepository.GetEmployeeByEmail(employeeCreateDto.Email);

            if (foundEmployee == null)
            {
                var employeeModel = _mapper.Map<Employee>(employeeCreateDto);

                employeeModel.Password = BCrypt.Net.BCrypt.HashPassword(employeeModel.Password);

                var registeredEmployee = _employeeRepository.RegisterEmployee(employeeModel);
                _employeeRepository.SaveChanges();

                AboutEmployeeCreateDto aboutEmployeeCreateDto = new AboutEmployeeCreateDto() { EmployeeId = registeredEmployee.EmployeeId };
                var aboutEmployeeModel = _mapper.Map<AboutEmployee>(aboutEmployeeCreateDto);
                _aboutEmployeeRepository.CreateAboutEmployee(aboutEmployeeModel);
                _aboutEmployeeRepository.SaveChanges();

                var employeeReadDto = _mapper.Map<EmployeeReadDto>(employeeModel);

                await _emailManagement.SendEmailForRegistiration(employeeModel.Name, employeeModel.Email);

                return Ok(employeeReadDto);
            }

            return Problem(title: "Unable to register.", detail: "Email already in use.");
        }
    }

}