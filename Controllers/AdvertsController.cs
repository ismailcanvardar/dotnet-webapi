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
        [HttpGet]
        public ActionResult<Advert> GetAdvert([FromBody] string externalId)
        {
            if (externalId == null)
            {
                return Problem(title: "Lack of information.", detail: "ExternalId is not provided.");
            }

            var advert = _repository.GetAdvert(externalId);

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
        [HttpDelete("{externalId}")]
        public ActionResult<bool> RemoveAdvert(string externalId)
        {
            if (IsEmployer())
            {
                var advert = _repository.GetAdvert(externalId);
                if (advert.EmployerId == GetCurrentUserExternalId())
                {
                    var isDeleted = _repository.RemoveAdvert(externalId);
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
        [HttpPost("apply/{advertExternalId}")]
        public ActionResult<Application> ApplyToAdvert(string advertExternalId)
        {
            if (IsUser())
            {
                var appliedAdvert = _repository.GetAdvert(advertExternalId);
                if (_repository.IsEmployeeApplied(GetCurrentUserExternalId(), appliedAdvert.ExternalId) == true)
                {
                    return Problem("Already applied.");
                } else
                {
                    var application = new Application { AdvertId = appliedAdvert.ExternalId, EmployerId = appliedAdvert.EmployerId, EmployeeId = GetCurrentUserExternalId() };
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
                var advert = _repository.GetAdvert(pickedEmployeeCreateDto.AdvertExternalId);

                if (advert.EmployerId == GetCurrentUserExternalId())
                {
                    var foundPickedEmployee = _repository.GetPickedEmployeeByAdvertAndEmployee(pickedEmployeeCreateDto.AdvertExternalId, pickedEmployeeCreateDto.EmployeeExternalId);

                    if (foundPickedEmployee == null)
                    {
                        var pickedEmployeeModel = _mapper.Map<PickedEmployee>(pickedEmployeeCreateDto);
                        pickedEmployeeModel.EmployerExternalId = GetCurrentUserExternalId();
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
                var advert = _repository.GetAdvert(pickedEmployeeRemoveDto.AdvertExternalId);

                if (advert.EmployerId == GetCurrentUserExternalId())
                {
                    var pickedEmployee = _repository.GetPickedEmployeeByAdvertAndEmployee(pickedEmployeeRemoveDto.AdvertExternalId, pickedEmployeeRemoveDto.EmployeeExternalId);

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
        [HttpDelete("apply/{externalId}")]
        public ActionResult CancelApplication(string externalId)
        {
            if (IsUser())
            {
                var canceledApplication = _repository.CancelApplication(externalId, GetCurrentUserExternalId());
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
        [HttpGet("getApplicationsByAdvert/{advertExternalId}")]
        public ActionResult<IEnumerable<Application>> GetApplicationsByAdvert(string advertExternalId)
        {
            if (IsEmployer())
            {
                var applications = _repository.GetApplicationsByAdvert(advertExternalId);
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

        public string GetCurrentUserExternalId()
        {
            var currentUser = HttpContext.User;
            var userExternalId = currentUser.Claims.FirstOrDefault(c => c.Type == "ExternalId").Value;

            if (userExternalId != null)
            {
                return userExternalId;
            }

            return null;
        }
    }
}
