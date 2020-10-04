using System;
using System.Collections.Generic;
using System.Linq;
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
    [Route("api/adverts")]
    [ApiController]
    public class AdvertsController : ControllerBase
    {
        private readonly IAdvertRepo _advertRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IAuthenticationHelper _authenticationHelper;

        public AdvertsController(IAdvertRepo advertRepository, IMapper mapper, IConfiguration config, IAuthenticationHelper authenticationHelper)
        {
            _advertRepository = advertRepository;
            _mapper = mapper;
            _config = config;
            _authenticationHelper = authenticationHelper;
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
        [HttpGet("withApps/{advertId}")]
        public ActionResult<IQueryable> GetAdvertWithApplication(Guid advertId)
        {
            if (advertId == null)
            {
                return Problem(title: "Lack of information.", detail: "ExternalId is not provided.");
            }

            var advert = _advertRepository.GetAdvertWithApplication(advertId);

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
            if (_authenticationHelper.IsEmployer())
            {
                var advertModel = _mapper.Map<Advert>(advertCreateDto);
                advertModel.EmployerId = _authenticationHelper.GetCurrentUserId();
                _advertRepository.CreateAdvert(advertModel);
                _advertRepository.SaveChanges();

                return Ok(_mapper.Map<AdvertReadDto>(advertModel));
            }

            return Problem(title: "Unable to react this route.", detail: "Insufficient permission.");
        }

        [Authorize]
        [HttpPost("search")]
        public ActionResult<IEnumerable<IQueryable>> SearchAdverts([FromBody] AdvertSearchDto advertSearchDto)
        {
            var adverts = _advertRepository.SearchAdverts(advertSearchDto);

            if (adverts == null)
            {
                return NotFound();
            }

            return Ok(adverts);
        }

        [Authorize]
        [HttpDelete("{advertId}")]
        public ActionResult<bool> RemoveAdvert(Guid advertId)
        {
            if (_authenticationHelper.IsEmployer())
            {
                var advert = _advertRepository.GetAdvert(advertId);
                if (advert.EmployerId == _authenticationHelper.GetCurrentUserId())
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
            if (_authenticationHelper.IsEmployer())
            {
                var adverts = _advertRepository.GetMyAdverts(_authenticationHelper.GetCurrentUserId());
                return Ok(adverts);
            }

            return Problem(title: "Unable to fetch.", detail: "Only employers can get their adverts.");
        }
    }
}
