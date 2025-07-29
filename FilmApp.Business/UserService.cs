using FilmApp.DataAccess;
using FilmApp.Entities;
using FilmApp.Business.Interfaces;

namespace FilmApp.Business.Services
{
    public class UserService : IUserService
    {
        private readonly FilmDbContext _dbContext;

        public UserService(FilmDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void RegisterUser(string username, string email, string password)
        {
            if (_dbContext.Users.Any(u => u.Username == username || u.Email == email))
                throw new Exception("Kullanıcı adı veya email zaten kayıtlı.");

            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }

        public User? LoginUser(string email, string plainPassword)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
                return null;

            bool passwordValid = BCrypt.Net.BCrypt.Verify(plainPassword, user.PasswordHash);
            return passwordValid ? user : null;
        }
    }
}