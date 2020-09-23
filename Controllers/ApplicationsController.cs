using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KariyerAppApi.Data;
using KariyerAppApi.Dtos;
using KariyerAppApi.Helpers;
using KariyerAppApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace KariyerAppApi.Controllers
{
    [Route("api/applications")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly IAdvertRepo _advertRepository;
        private readonly IApplicationRepo _applicationRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IAuthenticationHelper _authenticationHelper;
        private readonly IEmailManagement _emailManagement;
        private readonly IEmployerRepo _employerRepository;

        public ApplicationsController(
            IApplicationRepo applicationRepository,
            IMapper mapper, IConfiguration config,
            IAdvertRepo advertRepository,
            IAuthenticationHelper authenticationHelper,
            IEmailManagement emailManagement,
            IEmployerRepo employerRepository)
        {
            _advertRepository = advertRepository;
            _applicationRepository = applicationRepository;
            _mapper = mapper;
            _config = config;
            _authenticationHelper = authenticationHelper;
            _emailManagement = emailManagement;
            _employerRepository = employerRepository;
        }

        [Authorize]
        [HttpPost("apply/{advertId}")]
        public async Task<ActionResult<Application>> ApplyToAdvert(Guid advertId)
        {
            if (_authenticationHelper.IsEmployee())
            {
                var appliedAdvert = _advertRepository.GetAdvert(advertId);
                if (_applicationRepository.IsEmployeeApplied(_authenticationHelper.GetCurrentUserId(), appliedAdvert.AdvertId) == true)
                {
                    return Problem("Already applied.");
                }
                else
                {
                    var application = new Application { AdvertId = appliedAdvert.AdvertId, EmployerId = appliedAdvert.EmployerId, EmployeeId = _authenticationHelper.GetCurrentUserId() };
                    _applicationRepository.ApplyToAdvert(application);
                    _applicationRepository.SaveChanges();

                    _applicationRepository.ManageApplicantCount(application.AdvertId, ApplicantCountOperation.Increment);

                    var employer = _employerRepository.GetEmployer(appliedAdvert.EmployerId);
                    await _emailManagement.SendEmailToEmployerAboutApplication(appliedAdvert.Title, employer.Email);

                    return Ok(application);
                }
            }

            return Problem(title: "Unable to apply.", detail: "Only employees can apply.");
        }

        [Authorize]
        [HttpDelete("apply/{applicationId}")]
        public ActionResult CancelApplication(Guid applicationId)
        {
            if (_authenticationHelper.IsEmployee())
            {
                var canceledApplication = _applicationRepository.CancelApplication(applicationId, _authenticationHelper.GetCurrentUserId());
                if (canceledApplication != null)
                {
                    _applicationRepository.SaveChanges();
                    _applicationRepository.ManageApplicantCount(canceledApplication.AdvertId, ApplicantCountOperation.Decrement);

                    return Ok();
                }

                return Problem("Application is missed or not found.");
            }

            return Problem(title: "Unable to apply.", detail: "Only employees can cancel.");
        }

        [Authorize]
        [HttpGet("myApplications")]
        public ActionResult<IEnumerable<Application>> GetMyApplications()
        {
            if (_authenticationHelper.IsEmployee())
            {
                var applications = _applicationRepository.GetMyApplications(_authenticationHelper.GetCurrentUserId());
                return Ok(applications);
            }

            return Problem(title: "Unable to fetch.", detail: "Only employees can get their applications.");
        }

        [Authorize]
        [HttpGet("getApplicationsByAdvert/{advertId}")]
        public ActionResult<IEnumerable<Application>> GetApplicationsByAdvert(Guid advertId)
        {
            if (_authenticationHelper.IsEmployer())
            {
                var applications = _applicationRepository.GetApplicationsByAdvert(advertId);
                return Ok(applications);
            }

            return Problem(title: "Unable to fetch.", detail: "Only employers can get applications for their adverts.");
        }
    }
}
