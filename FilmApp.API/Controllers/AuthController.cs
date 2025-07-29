using FilmApp.API.Helpers;
using FilmApp.Business.Interfaces;
using FilmApp.Entities;
using FilmApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmApp.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public AuthController(IUserService userService, JwtTokenGenerator jwtTokenGenerator)
        {
            _userService = userService;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        /// <summary>
        /// Yeni kullanıcı kaydı yapar.
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            try
            {
                _userService.RegisterUser(request.Username, request.Email, request.Password);
                return Ok("User registered successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Registration failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Kullanıcı giriş yapar ve JWT token alır.
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 401)]
        public IActionResult Login([FromBody] UserLoginRequest request)
        {
            var user = _userService.LoginUser(request.Email, request.Password);

            if (user == null)
                return Unauthorized("Invalid email or password.");

            var token = _jwtTokenGenerator.GenerateToken(user);
            return Ok(new { Token = token });
        }
    }
}