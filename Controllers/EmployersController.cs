﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KariyerAppApi.Data;
using KariyerAppApi.Helpers;
using KariyerAppApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace KariyerAppApi.Controllers
{
    [Route("api/employers")]
    [ApiController]
    public class EmployersController : ControllerBase
    {
        private readonly IEmployerRepo _employerRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IAuthenticationHelper _authenticationHelper;
        private readonly IEmailManagement _emailManagement;

        public EmployersController(IEmployerRepo employerRepository, IMapper mapper, IConfiguration config, IAuthenticationHelper authenticationHelper, IEmailManagement emailManagement)
        {
            _employerRepository = employerRepository;
            _mapper = mapper;
            _config = config;
            _authenticationHelper = authenticationHelper;
            _emailManagement = emailManagement;
        }

        [Authorize]
        [HttpGet("getMyProfile")]
        public ActionResult<Employer> GetMyProfile()
        {
            Guid employerId = _authenticationHelper.GetCurrentUserId();

            return _employerRepository.GetEmployer(employerId);
        }

        [HttpGet("login/{email}/{password}")]
        public ActionResult<EmployerLoginDto> LoginEmployer(string email, string password)
        {
            Employer employer = _employerRepository.GetEmployerByEmail(email);

            // Controls if there is a user with the given email or hashed password is true
            // against given password from parameters
            if (employer == null || !BCrypt.Net.BCrypt.Verify(password, employer.Password))
            {
                return Problem(detail: "Username or password incorrect", statusCode: 500, title: "Wrong Credentials");
            }

            var employerLoginDto = _mapper.Map<EmployerLoginDto>(employer);

            employerLoginDto.Token = _authenticationHelper.GenerateJSONWebTokenForEmployer(employer);

            return Ok(employerLoginDto);
        }

        [HttpGet("findEmployer/{employerId}")]
        public ActionResult GetEmployer(Guid employerId)
        {
            var foundEmployer = _employerRepository.GetEmployer(employerId);

            if (foundEmployer == null)
            {
                return NotFound();
            }

            return Ok(foundEmployer);
        }

        [HttpGet("findEmployers/{searchCriteria}/{offset}/{limit}")]
        public ActionResult<IEnumerable<Employer>> SearchEmployers(string searchCriteria, int offset, int limit)
        {
            var employers = _employerRepository.SearchEmployers(searchCriteria, offset, limit);

            return Ok(employers);
        }

        [HttpPost]
        public async Task<ActionResult> RegisterEmployer([FromBody] EmployerCreateDto employerCreateDto)
        {
            var foundEmployer = _employerRepository.GetEmployerByEmail(employerCreateDto.Email);

            if (foundEmployer == null)
            {
                var employerModel = _mapper.Map<Employer>(employerCreateDto);

                employerModel.Password = BCrypt.Net.BCrypt.HashPassword(employerModel.Password);

                _employerRepository.RegisterEmployer(employerModel);
                _employerRepository.SaveChanges();

                var employerReadDto = _mapper.Map<EmployerReadDto>(employerModel);

                await _emailManagement.SendEmailForRegistiration(employerModel.Name, employerModel.Email);

                return Ok(employerReadDto);
            }

            return Problem(title: "Unable to register.", detail: "Email already in use.");
        }

        
    }
}
