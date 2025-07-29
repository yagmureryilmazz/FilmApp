using FilmApp.Entities;

namespace FilmApp.Business.Interfaces
{
    public interface IUserService
    {
        void RegisterUser(string username, string email, string password);
        User? LoginUser(string email, string plainPassword);
    }
}