using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FilmApp.DataAccess;
using FilmApp.Entities;
using FilmApp.API.Helpers;

namespace FilmApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly FilmDbContext _context;

        public UsersController(FilmDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Token'daki email bilgisinden mevcut kullanıcıyı getirir.
        /// </summary>
        private User GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
                throw new UnauthorizedAccessException("User not found.");

            return user;
        }

        /// <summary>
        /// Belirli bir kullanıcıyı getirir (sadece kendi bilgilerine erişebilir).
        /// </summary>
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var currentUser = GetCurrentUser();

            if (currentUser.Id != id)
                return Forbid("You are not authorized to access this user's data.");

            return Ok(new
            {
                currentUser.Id,
                currentUser.Username,
                currentUser.Email
            });
        }

        /// <summary>
        /// Kullanıcı bilgilerini günceller. Şifre alanı opsiyoneldir.
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UpdateUserRequest updatedUser)
        {
            var currentUser = GetCurrentUser();

            if (currentUser.Id != id)
                return Forbid("You are not authorized to update this user.");

            currentUser.Username = updatedUser.Username;
            currentUser.Email = updatedUser.Email;

            if (!string.IsNullOrWhiteSpace(updatedUser.Password))
            {
                currentUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updatedUser.Password);
            }

            _context.SaveChanges();
            return Ok("User updated successfully.");
        }
    }

    public class UpdateUserRequest
    {
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string? Password { get; set; }
    }
}
