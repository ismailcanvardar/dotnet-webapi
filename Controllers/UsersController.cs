using System.Collections.Generic;
using AutoMapper;
using Commander.Data;
using Commander.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System;
using Microsoft.Extensions.Configuration;

namespace Commander.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepo _repository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public UsersController(IUserRepo repository, IMapper mapper, IConfiguration config)
        {
            _repository = repository;
            _mapper = mapper;
            _config = config;
        }

        [HttpGet("login/{email}/{password}")]
        public ActionResult<UserLoginDto> LoginUser(string email, string password)
        {
            User user = _repository.LoginUser(email, password);

            if (user == null)
            {
                return Problem(detail: "Username or password incorrect", statusCode: 500, title: "Wrong Credentials");
            }

            var userLoginDto = _mapper.Map<UserLoginDto>(user);

            userLoginDto.Token = GenerateJSONWebToken(email, password);

            return Ok(userLoginDto);
        }

        [Authorize]
        [HttpGet("findUser/{username}")]
        public ActionResult GetUserByUsername(string username)
        {
            var foundUser = _repository.GetUserByUsername(username);

            if (foundUser == null)
            {
                return NotFound();
            }

            return Ok(foundUser);
        }

        [AllowAnonymous]
        [HttpGet("findUsers/{username}/{offset}")]
        public ActionResult<IEnumerable<UserReadDto>> SearchUsersByUsername(string username, int offset)
        {
            var foundUsers = _repository.SearchUsersByUsername(username, offset);

            return Ok(_mapper.Map<IEnumerable<UserReadDto>>(foundUsers));
        }

        [HttpPost]
        public ActionResult RegisterUser([FromBody] UserCreateDto userCreteDto)
        {
            var userModel = _mapper.Map<User>(userCreteDto);
            _repository.RegisterUser(userModel);
            _repository.SaveChanges();

            var userReadDto = _mapper.Map<UserReadDto>(userModel);

            return Ok(userReadDto);
        }

        private string GenerateJSONWebToken(string email, string password)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "12"),
                new Claim(JwtRegisteredClaimNames.Email, email)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}