using System.Security.Claims;
using FilmApp.DataAccess;
using FilmApp.Entities;
using Microsoft.EntityFrameworkCore;
using FilmApp.Business.Interfaces;

namespace FilmApp.Business.Services
{
    public class WatchHistoryService : IWatchHistoryService
    {
        private readonly FilmDbContext _context;

        public WatchHistoryService(FilmDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Token içindeki email claim'ini okuyarak kullanıcı ID'sini döner.
        /// </summary>
        private int GetUserIdFromClaims(ClaimsPrincipal userClaims)
        {
            var email = userClaims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                throw new UnauthorizedAccessException("Token içinde email bulunamadı.");

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
                throw new UnauthorizedAccessException("Kullanıcı veritabanında bulunamadı.");

            return user.Id;
        }

        /// <summary>
        /// Kullanıcının izleme geçmişine film ekler. Tekrar izleme kontrolü yapar.
        /// </summary>
        public void AddToWatchHistory(int userIdFromRoute, int filmId, ClaimsPrincipal userClaims)
        {
            var tokenUserId = GetUserIdFromClaims(userClaims);
            if (tokenUserId != userIdFromRoute)
                throw new UnauthorizedAccessException("Bu kullanıcıya işlem yapma yetkiniz yok.");

            var filmExists = _context.Films.Any(f => f.Id == filmId);
            if (!filmExists)
                throw new KeyNotFoundException($"ID'si {filmId} olan film bulunamadı.");

            var alreadyWatched = _context.WatchHistories
                .Any(w => w.UserId == userIdFromRoute && w.FilmId == filmId);

            if (alreadyWatched)
                throw new InvalidOperationException("Bu film zaten izlenmiş.");

            var watchEntry = new WatchHistory
            {
                UserId = userIdFromRoute,
                FilmId = filmId,
                WatchedDate = DateTime.Now
            };

            _context.WatchHistories.Add(watchEntry);
            _context.SaveChanges();
        }

        /// <summary>
        /// Kullanıcının izleme geçmişini listeler.
        /// </summary>
        public List<WatchHistory> GetWatchHistory(int userIdFromRoute, ClaimsPrincipal userClaims)
        {
            var tokenUserId = GetUserIdFromClaims(userClaims);
            if (tokenUserId != userIdFromRoute)
                throw new UnauthorizedAccessException("Bu kullanıcıya erişim yetkiniz yok.");

            var history = _context.WatchHistories
                .Include(w => w.Film)
                .Where(w => w.UserId == userIdFromRoute)
                .ToList();

            return history;
        }
    }
}


