using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Linq;
using FilmApp.DataAccess;
using FilmApp.Entities;

namespace FilmApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users/{userId}/watched")]
    public class WatchHistoryController : ControllerBase
    {
        private readonly FilmDbContext _context;

        public WatchHistoryController(FilmDbContext context)
        {
            _context = context;
        }

        // ✅ Kullanıcı ID'sini token'daki email'den bulma
        private int GetUserIdFromToken()
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (emailClaim == null)
                throw new UnauthorizedAccessException("Email not found in token.");

            var email = emailClaim.Value;

            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
                throw new UnauthorizedAccessException("User not found in database.");

            return user.Id;
        }

        // ✅ İzleme geçmişine film ekleme (TEKRAR EKLENMESİN kontrolüyle)
        [HttpPost("{filmId}")]
        public IActionResult AddToWatchHistory(int userId, int filmId)
        {
            int tokenUserId = GetUserIdFromToken();
            if (tokenUserId != userId)
                return StatusCode(403, "You are not authorized to access this resource.");

            var filmExists = _context.Films.Any(f => f.Id == filmId);
            if (!filmExists)
                return NotFound($"Film with ID {filmId} not found.");

            var alreadyWatched = _context.WatchHistories
                .Any(w => w.UserId == userId && w.FilmId == filmId);

            if (alreadyWatched)
                return BadRequest("This film is already in the user's watch history.");

            var watchEntry = new WatchHistory
            {
                UserId = userId,
                FilmId = filmId,
                WatchedDate = DateTime.Now
            };

            _context.WatchHistories.Add(watchEntry);
            _context.SaveChanges();

            return Ok(watchEntry);
        }

        // ✅ İzleme geçmişini listeleme
        [HttpGet]
        public IActionResult GetWatchHistory(int userId)
        {
            int tokenUserId = GetUserIdFromToken();
            if (tokenUserId != userId)
                return Forbid("You are not authorized to access this resource.");

            var history = _context.WatchHistories
                .Include(w => w.Film)
                .Where(w => w.UserId == userId)
                .ToList();

            return Ok(history);
        }
    }
}

