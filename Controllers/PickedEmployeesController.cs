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
    [Route("api/pickedEmployees")]
    [ApiController]
    public class PickedEmployeesController : ControllerBase
    {
        private readonly IPickedEmployeeRepo _pickedEmployeeRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IAdvertRepo _advertRepository;

        public PickedEmployeesController(IPickedEmployeeRepo pickedEmployeeRepository, IMapper mapper, IConfiguration config, IAdvertRepo advertRepository)
        {
            _pickedEmployeeRepository = pickedEmployeeRepository;
            _mapper = mapper;
            _config = config;
            _advertRepository = advertRepository;
        }

        [Authorize]
        [HttpPost("pickEmployee")]
        public ActionResult<PickedEmployee> PickEmployee([FromBody] PickedEmployeeCreateDto pickedEmployeeCreateDto)
        {
            if (IsEmployer())
            {
                var advert = _advertRepository.GetAdvert(pickedEmployeeCreateDto.AdvertId);

                if (advert.EmployerId == GetCurrentUserExternalId())
                {
                    var foundPickedEmployee = _pickedEmployeeRepository.GetPickedEmployeeByAdvertAndEmployee(pickedEmployeeCreateDto.AdvertId, pickedEmployeeCreateDto.EmployeeId);

                    if (foundPickedEmployee == null)
                    {
                        var pickedEmployeeModel = _mapper.Map<PickedEmployee>(pickedEmployeeCreateDto);
                        pickedEmployeeModel.EmployerId = GetCurrentUserExternalId();
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
            if (IsEmployer())
            {
                var advert = _advertRepository.GetAdvert(pickedEmployeeRemoveDto.AdvertId);

                if (advert.EmployerId == GetCurrentUserExternalId())
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
