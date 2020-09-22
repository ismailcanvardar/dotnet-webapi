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
    [Route("api/pickedEmployees")]
    [ApiController]
    public class PickedEmployeesController : ControllerBase
    {
        private readonly IPickedEmployeeRepo _pickedEmployeeRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IAdvertRepo _advertRepository;
        private readonly IAuthenticationHelper _authenticationHelper;

        public PickedEmployeesController(IPickedEmployeeRepo pickedEmployeeRepository, IMapper mapper, IConfiguration config, IAdvertRepo advertRepository, IAuthenticationHelper authenticationHelper)
        {
            _pickedEmployeeRepository = pickedEmployeeRepository;
            _mapper = mapper;
            _config = config;
            _advertRepository = advertRepository;
            _authenticationHelper = authenticationHelper;
        }

        [Authorize]
        [HttpPost("pickEmployee")]
        public ActionResult<PickedEmployee> PickEmployee([FromBody] PickedEmployeeCreateDto pickedEmployeeCreateDto)
        {
            if (_authenticationHelper.IsEmployer())
            {
                var advert = _advertRepository.GetAdvert(pickedEmployeeCreateDto.AdvertId);

                if (advert.EmployerId == _authenticationHelper.GetCurrentUserId())
                {
                    var foundPickedEmployee = _pickedEmployeeRepository.GetPickedEmployeeByAdvertAndEmployee(pickedEmployeeCreateDto.AdvertId, pickedEmployeeCreateDto.EmployeeId);

                    if (foundPickedEmployee == null)
                    {
                        var pickedEmployeeModel = _mapper.Map<PickedEmployee>(pickedEmployeeCreateDto);
                        pickedEmployeeModel.EmployerId = _authenticationHelper.GetCurrentUserId();
                        _pickedEmployeeRepository.PickEmployee(pickedEmployeeModel);
                        _pickedEmployeeRepository.SaveChanges();
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
            if (_authenticationHelper.IsEmployer())
            {
                var advert = _advertRepository.GetAdvert(pickedEmployeeRemoveDto.AdvertId);

                if (advert.EmployerId == _authenticationHelper.GetCurrentUserId())
                {
                    var pickedEmployee = _pickedEmployeeRepository.GetPickedEmployeeByAdvertAndEmployee(pickedEmployeeRemoveDto.AdvertId, pickedEmployeeRemoveDto.EmployeeId);

                    if (pickedEmployee != null)
                    {
                        _pickedEmployeeRepository.UnpickEmployee(pickedEmployee);
                        _pickedEmployeeRepository.SaveChanges();
                        return Ok();
                    }

                    return Problem(title: "Unable to pick.", detail: "Already unpicked.");
                }

                return Problem(title: "Unable to pick.", detail: "Only employers can unpick.");
            }

            return Problem(title: "Unable to pick.", detail: "Only employers can unpick.");
        }
    }
}
