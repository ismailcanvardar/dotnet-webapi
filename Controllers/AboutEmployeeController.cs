using System;
using System.Linq;
using AutoMapper;
using KariyerAppApi.Data;
using KariyerAppApi.Dtos;
using KariyerAppApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace KariyerAppApi.Controllers
{
    [Route("api/aboutEmployee")]
    [ApiController]
    public class AboutEmployeeController : ControllerBase
    {
        private readonly IAboutEmployeeRepo _aboutEmployeeRepo;
        private readonly BaseContext _context;
        private readonly IMapper _mapper;

        public AboutEmployeeController(IAboutEmployeeRepo aboutEmployeeRepo, BaseContext context, IMapper mapper)
        {
            _aboutEmployeeRepo = aboutEmployeeRepo;
            _context = context;
            _mapper = mapper;
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        [HttpGet("{employeeId}")]
        public ActionResult GetAboutEmployee(Guid employeeId)
        {
            var aboutEmployee = _aboutEmployeeRepo.GetAboutEmployee(employeeId);

            if (aboutEmployee != null)
            {
                return Ok(aboutEmployee);
            }

            return Problem();
        }

        [HttpPost]
        public ActionResult UpdateAboutEmployee([FromBody] AboutEmployeeUpdateDto aboutEmployeeUpdateDto)
        {
            var foundAboutEmployee = _context.AboutEmployees.FirstOrDefault(ae => ae.EmployeeId == aboutEmployeeUpdateDto.EmployeeId);

            if (foundAboutEmployee == null)
            {
                return Problem();
            }

            if (aboutEmployeeUpdateDto.Job != null && aboutEmployeeUpdateDto.BriefInformation != null)
            {
                foundAboutEmployee.Job = aboutEmployeeUpdateDto.Job;
                foundAboutEmployee.BriefInformation = aboutEmployeeUpdateDto.BriefInformation;
                SaveChanges();
            } else if (aboutEmployeeUpdateDto.BriefInformation != null)
            {
                foundAboutEmployee.BriefInformation = aboutEmployeeUpdateDto.BriefInformation;
                SaveChanges();
            } else if (aboutEmployeeUpdateDto.Job != null)
            {
                foundAboutEmployee.Job = aboutEmployeeUpdateDto.Job;
                SaveChanges();
            }

            return Ok();
        }
    }
}

