using System;
using System.Linq;
using AutoMapper;
using KariyerAppApi.Data;
using KariyerAppApi.Dtos;
using KariyerAppApi.Helpers;
using KariyerAppApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace KariyerAppApi.Controllers
{
    [Route("api/ratings")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingRepo _ratingRepository;
        private readonly IMapper _mapper;
        private readonly IAdvertRepo _advertRepository;
        private readonly IPickedEmployeeRepo _pickedEmployeeRepository;
        private readonly IAuthenticationHelper _authenticationHelper;

        public RatingsController(IRatingRepo ratingRepository, IMapper mapper, IConfiguration config, IAdvertRepo advertRepository, IPickedEmployeeRepo pickedEmployeeRepository, IAuthenticationHelper authenticationHelper)
        {
            _ratingRepository = ratingRepository;
            _mapper = mapper;
            _advertRepository = advertRepository;
            _pickedEmployeeRepository = pickedEmployeeRepository;
            _authenticationHelper = authenticationHelper;
        }

        [Authorize]
        [HttpPost("rate")]
        public ActionResult<Rating> RateEmployee(RatingCreateDto ratingCreateDto)
        {
            if (_authenticationHelper.IsEmployer())
            {
                var pickedEmployee = _pickedEmployeeRepository.GetPickedEmployeeByAdvertAndEmployee(ratingCreateDto.AdvertId, ratingCreateDto.EmployeeId);

                if (pickedEmployee != null)
                {
                    var ratingExists = _ratingRepository.GetRating(ratingCreateDto.EmployeeId, ratingCreateDto.AdvertId);

                    if (ratingExists == null)
                    {
                        var ratingModel = _mapper.Map<Rating>(ratingCreateDto);
                        ratingModel.EmployerId = _authenticationHelper.GetCurrentUserId();
                        _ratingRepository.RateEmployee(ratingModel);
                        return Ok(ratingModel);
                    }

                    return Problem("Unable to rate.", "Already rated.");
                }

                return Problem("Unable to rate.", "Pick employee first.");
            }

            return Problem("Unable to rate.", "Only employers can rate employees");
        }
    }
}
