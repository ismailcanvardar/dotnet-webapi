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
            User user = _repository.GetUserByEmail(email);

            // Controls if there is a user with the given email or hashed password is true
            // against given password from parameters
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return Problem(detail: "Username or password incorrect", statusCode: 500, title: "Wrong Credentials");
            }

            var userLoginDto = _mapper.Map<UserLoginDto>(user);

            userLoginDto.Token = GenerateJSONWebTokenForUser(user);

            return Ok(userLoginDto);
        }

        [HttpGet("findUser/{externalId}")]
        public ActionResult GetUser(string externalId)
        {
            var foundUser = _repository.GetUser(externalId);

            if (foundUser == null)
            {
                return NotFound();
            }

            return Ok(foundUser);
        }

        [HttpGet("findUsers/{searchCriteria}/{offset}/{limit}")]
        public ActionResult<IEnumerable<UserReadDto>> SearchUser(string searchCriteria, int offset, int limit)
        {
            var foundUsers = _repository.SearchUser(searchCriteria, offset, limit);

            return Ok(_mapper.Map<IEnumerable<UserReadDto>>(foundUsers));
        }

        [HttpPost]
        public ActionResult RegisterUser([FromBody] UserCreateDto userCreateDto)
        {
            var userModel = _mapper.Map<User>(userCreateDto);

            userModel.Password = BCrypt.Net.BCrypt.HashPassword(userModel.Password);

            _repository.RegisterUser(userModel);
            _repository.SaveChanges();

            var userReadDto = _mapper.Map<UserReadDto>(userModel);

            return Ok(userReadDto);
        }

        private string GenerateJSONWebTokenForUser(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("Role", "User"),
                new Claim("ExternalId", user.ExternalId)
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