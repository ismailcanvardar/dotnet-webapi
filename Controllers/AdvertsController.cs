using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Commander.Data;
using Commander.Dtos;
using Commander.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Commander.Controllers
{
    [Route("api/adverts")]
    [ApiController]
    public class AdvertsController : ControllerBase
    {
        private readonly IAdvertRepo _repository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public AdvertsController(IAdvertRepo repository, IMapper mapper, IConfiguration config)
        {
            _repository = repository;
            _mapper = mapper;
            _config = config;
        }

        [Authorize]
        [HttpGet("{advertId}")]
        public ActionResult<Advert> GetAdvert(Guid advertId)
        {
            if (advertId == null)
            {
                return Problem(title: "Lack of information.", detail: "ExternalId is not provided.");
            }

            var advert = _repository.GetAdvert(advertId);

            if (advert == null)
            {
                return NotFound();
            }

            return Ok(advert);
        }

        [Authorize]
        [HttpPost]
        public ActionResult<AdvertReadDto> CreateAdvert(AdvertCreateDto advertCreateDto)
        {
            if (IsEmployer())
            {
                var advertModel = _mapper.Map<Advert>(advertCreateDto);
                advertModel.EmployerId = GetCurrentUserExternalId();
                _repository.CreateAdvert(advertModel);
                _repository.SaveChanges();

                return Ok(_mapper.Map<AdvertReadDto>(advertModel));
            }

            return Problem(title: "Unable to react this route.", detail: "Insufficient permission.");
        }

        [Authorize]
        [HttpGet("search")]
        public ActionResult<IEnumerable<Advert>> SearchAdverts(AdvertSearchDto advertSearchDto)
        {
            var adverts = _repository.SearchAdverts(advertSearchDto);

            if (adverts.Count() == 0 || adverts == null)
            {
                NotFound();
            }

            return Ok(adverts);
        }

        [Authorize]
        [HttpDelete("{advertId}")]
        public ActionResult<bool> RemoveAdvert(Guid advertId)
        {
            if (IsEmployer())
            {
                var advert = _repository.GetAdvert(advertId);
                if (advert.EmployerId == GetCurrentUserExternalId())
                {
                    var isDeleted = _repository.RemoveAdvert(advertId);
                    if (isDeleted)
                    {
                        _repository.SaveChanges();
                        return Ok();
                    }
                    else
                    {
                        return Problem(title: "Something went wrong.", statusCode: 500);
                    }
                } else
                {
                    return Problem(title: "Unable to delete advert.", detail: "You cannot remove advert that is not belonged to you.");
                }
            }

            return Problem(title: "Unable to reach this route.", detail: "Insufficient permission.");
        }

        [Authorize]
        [HttpPost("apply/{advertId}")]
        public ActionResult<Application> ApplyToAdvert(Guid advertId)
        {
            if (IsUser())
            {
                var appliedAdvert = _repository.GetAdvert(advertId);
                if (_repository.IsEmployeeApplied(GetCurrentUserExternalId(), appliedAdvert.AdvertId) == true)
                {
                    return Problem("Already applied.");
                } else
                {
                    var application = new Application { AdvertId = appliedAdvert.AdvertId, EmployerId = appliedAdvert.EmployerId, EmployeeId = GetCurrentUserExternalId() };
                    _repository.ApplyToAdvert(application);
                    _repository.SaveChanges();

                    _repository.ManageApplicantCount(application.AdvertId, ApplicantCountOperation.Increment);

                    return Ok(application);
                }
            }

            return Problem(title: "Unable to apply.", detail: "Only users can apply.");
        }

        [Authorize]
        [HttpPost("pickEmployee")]
        public ActionResult<PickedEmployee> PickEmployee([FromBody] PickedEmployeeCreateDto pickedEmployeeCreateDto)
        {
            if (IsEmployer())
            {
                var advert = _repository.GetAdvert(pickedEmployeeCreateDto.AdvertId);

                if (advert.EmployerId == GetCurrentUserExternalId())
                {
                    var foundPickedEmployee = _repository.GetPickedEmployeeByAdvertAndEmployee(pickedEmployeeCreateDto.AdvertId, pickedEmployeeCreateDto.EmployeeId);

                    if (foundPickedEmployee == null)
                    {
                        var pickedEmployeeModel = _mapper.Map<PickedEmployee>(pickedEmployeeCreateDto);
                        pickedEmployeeModel.EmployerId = GetCurrentUserExternalId();
                        _repository.PickEmployee(pickedEmployeeModel);
                        _repository.SaveChanges();
                        return Ok(pickedEmployeeModel);
                    }

                    return Problem(title: "Already picked.", detail: "This employee already picked.");
                }

                return Problem(title: "Unable to pick.", detail: "Only advert owner can pick employee");
            }

            return Problem(title: "Unable to pick.", detail: "Only employers can pick.");
        }

        [Authorize]
        [HttpDelete("unpickEmployee")]
        public ActionResult UnpickEmployee([FromBody] PickedEmployeeRemoveDto pickedEmployeeRemoveDto)
        {
            if (IsEmployer())
            {
                var advert = _repository.GetAdvert(pickedEmployeeRemoveDto.AdvertId);

                if (advert.EmployerId == GetCurrentUserExternalId())
                {
                    var pickedEmployee = _repository.GetPickedEmployeeByAdvertAndEmployee(pickedEmployeeRemoveDto.AdvertId, pickedEmployeeRemoveDto.EmployeeId);

                    if (pickedEmployee != null)
                    {
                        _repository.UnpickEmployee(pickedEmployee);
                        _repository.SaveChanges();
                        return Ok();
                    }

                    return Problem(title: "Unable to pick.", detail: "Already unpicked.");
                }

                return Problem(title: "Unable to pick.", detail: "Only employers can unpick.");
            }

            return Problem(title: "Unable to pick.", detail: "Only employers can unpick.");
        }

        [Authorize]
        [HttpDelete("apply/{applicationId}")]
        public ActionResult CancelApplication(Guid applicationId)
        {
            if (IsUser())
            {
                var canceledApplication = _repository.CancelApplication(applicationId, GetCurrentUserExternalId());
                if (canceledApplication != null)
                {
                    _repository.SaveChanges();
                    _repository.ManageApplicantCount(canceledApplication.AdvertId, ApplicantCountOperation.Decrement);

                    return Ok();
                }

                return Problem("Application is missed or not found.");
            }

            return Problem(title: "Unable to apply.", detail: "Only users can cancel.");
        }

        [Authorize]
        [HttpGet("myAdverts")]
        public ActionResult<IEnumerable<Advert>> GetMyAdverts()
        {
            if (IsEmployer())
            {
                var adverts = _repository.GetMyAdverts(GetCurrentUserExternalId());
                return Ok(adverts);
            }

            return Problem(title: "Unable to fetch.", detail: "Only employers can get their adverts.");
        }

        [Authorize]
        [HttpGet("myApplications")]
        public ActionResult<IEnumerable<Application>> GetMyApplications()
        {
            if (IsUser())
            {
                var applications = _repository.GetMyApplications(GetCurrentUserExternalId());
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
                var applications = _repository.GetApplicationsByAdvert(advertId);
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
