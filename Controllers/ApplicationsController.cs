using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using KariyerAppApi.Data;
using KariyerAppApi.Dtos;
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

        public ApplicationsController(IApplicationRepo applicationRepository, IMapper mapper, IConfiguration config, IAdvertRepo advertRepository)
        {
            _advertRepository = advertRepository;
            _applicationRepository = applicationRepository;
            _mapper = mapper;
            _config = config;
        }

        [Authorize]
        [HttpPost("apply/{advertId}")]
        public ActionResult<Application> ApplyToAdvert(Guid advertId)
        {
            if (IsUser())
            {
                var appliedAdvert = _advertRepository.GetAdvert(advertId);
                if (_applicationRepository.IsEmployeeApplied(GetCurrentUserExternalId(), appliedAdvert.AdvertId) == true)
                {
                    return Problem("Already applied.");
                }
                else
                {
                    var application = new Application { AdvertId = appliedAdvert.AdvertId, EmployerId = appliedAdvert.EmployerId, EmployeeId = GetCurrentUserExternalId() };
                    _applicationRepository.ApplyToAdvert(application);
                    _applicationRepository.SaveChanges();

                    _applicationRepository.ManageApplicantCount(application.AdvertId, ApplicantCountOperation.Increment);

                    return Ok(application);
                }
            }

            return Problem(title: "Unable to apply.", detail: "Only users can apply.");
        }

        [Authorize]
        [HttpDelete("apply/{applicationId}")]
        public ActionResult CancelApplication(Guid applicationId)
        {
            if (IsUser())
            {
                var canceledApplication = _applicationRepository.CancelApplication(applicationId, GetCurrentUserExternalId());
                if (canceledApplication != null)
                {
                    _applicationRepository.SaveChanges();
                    _applicationRepository.ManageApplicantCount(canceledApplication.AdvertId, ApplicantCountOperation.Decrement);

                    return Ok();
                }

                return Problem("Application is missed or not found.");
            }

            return Problem(title: "Unable to apply.", detail: "Only users can cancel.");
        }

        [Authorize]
        [HttpGet("myApplications")]
        public ActionResult<IEnumerable<Application>> GetMyApplications()
        {
            if (IsUser())
            {
                var applications = _applicationRepository.GetMyApplications(GetCurrentUserExternalId());
                return Ok(applications);
            }

            return Problem(title: "Unable to fetch.", detail: "Only users can get their applications.");
        }

        [Authorize]
        [HttpGet("getApplicationsByAdvert/{advertId}")]
        public ActionResult<IEnumerable<Application>> GetApplicationsByAdvert(Guid advertId)
        {
            if (IsEmployer())
            {
                var applications = _applicationRepository.GetApplicationsByAdvert(advertId);
                return Ok(applications);
            }

            return Problem(title: "Unable to fetch.", detail: "Only employers can get applications for their adverts.");
        }

        public bool IsEmployer()
        {
            var currentUser = HttpContext.User;
            var userRole = currentUser.Claims.FirstOrDefault(c => c.Type == "Role").Value;

            if (userRole == "Employer")
            {
                return true;
            }

            return false;
        }

        public bool IsUser()
        {
            var currentUser = HttpContext.User;
            var userRole = currentUser.Claims.FirstOrDefault(c => c.Type == "Role").Value;

            if (userRole == "User")
            {
                return true;
            }

            return false;
        }

        public Guid GetCurrentUserExternalId()
        {
            var currentUser = HttpContext.User;
            var currentUserId = currentUser.Claims.FirstOrDefault(c => c.Type == "CurrentUserId").Value;

            return Guid.Parse(currentUserId);
        }
    }
}
