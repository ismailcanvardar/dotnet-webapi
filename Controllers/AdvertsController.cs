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
        private readonly IAdvertRepo _advertRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public AdvertsController(IAdvertRepo advertRepository, IMapper mapper, IConfiguration config)
        {
            _advertRepository = advertRepository;
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

            var advert = _advertRepository.GetAdvert(advertId);

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
                _advertRepository.CreateAdvert(advertModel);
                _advertRepository.SaveChanges();

                return Ok(_mapper.Map<AdvertReadDto>(advertModel));
            }

            return Problem(title: "Unable to react this route.", detail: "Insufficient permission.");
        }

        [Authorize]
        [HttpGet("search")]
        public ActionResult<IEnumerable<Advert>> SearchAdverts(AdvertSearchDto advertSearchDto)
        {
            var adverts = _advertRepository.SearchAdverts(advertSearchDto);

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
                var advert = _advertRepository.GetAdvert(advertId);
                if (advert.EmployerId == GetCurrentUserExternalId())
                {
                    var isDeleted = _advertRepository.RemoveAdvert(advertId);
                    if (isDeleted)
                    {
                        _advertRepository.SaveChanges();
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
        [HttpGet("myAdverts")]
        public ActionResult<IEnumerable<Advert>> GetMyAdverts()
        {
            if (IsEmployer())
            {
                var adverts = _advertRepository.GetMyAdverts(GetCurrentUserExternalId());
                return Ok(adverts);
            }

            return Problem(title: "Unable to fetch.", detail: "Only employers can get their adverts.");
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
