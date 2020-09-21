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
        [HttpPost("apply")]
        public ActionResult<Application> ApplyToAdvert([FromBody] ApplicationCreateDto applicationCreateDto)
        {
            if (IsUser())
            {
                if (_repository.IsEmployeeApplied(GetCurrentUserExternalId(), applicationCreateDto.AdvertId) == true)
                {
                    return Problem("Already applied.");
                } else
                {
                    var applicationModel = _mapper.Map<Application>(applicationCreateDto);
                    applicationModel.EmployeeId = GetCurrentUserExternalId();
                    _repository.ApplyToAdvert(applicationModel);
                    _repository.SaveChanges();

                    return Ok(applicationModel);
                }
            }

            return Problem(title: "Unable to apply.", detail: "Only users can apply.");
        }

        [Authorize]
        [HttpDelete("apply/{externalId}")]
        public ActionResult CancelApplication(string externalId)
        {
            if (IsUser())
            {
                var isCanceled = _repository.CancelApplication(externalId, GetCurrentUserExternalId());
                if (isCanceled == true)
                {
                    _repository.SaveChanges();
                    return Ok();
                }

                return Problem("Application is missed or not found.");
            }

            return Problem(title: "Unable to apply.", detail: "Only users can cancel.");
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
