using FilmApp.Entities;
using System.Security.Claims;

namespace FilmApp.Business.Interfaces
{
    public interface IWatchHistoryService
    {
        void AddToWatchHistory(int userIdFromRoute, int filmId, ClaimsPrincipal userClaims);
        List<WatchHistory> GetWatchHistory(int userIdFromRoute, ClaimsPrincipal userClaims);
    }
}