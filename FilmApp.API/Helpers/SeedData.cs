using FilmApp.DataAccess;
using FilmApp.Entities;
using FilmApp.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace FilmApp.API.Data
{
    public static class SeedData
    {
        public static void EnsureSeeded(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<FilmDbContext>();

            context.Database.Migrate();

            // ✅ Demo kullanıcı
            if (!context.Users.Any())
            {
                var user = new User
                {
                    Username = "demo",
                    Email = "demo@filmapp.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("1234")
                };

                context.Users.Add(user);
                context.SaveChanges(); // ID alması için kaydet
            }

            var demoUser = context.Users.FirstOrDefault(u => u.Email == "demo@filmapp.com");

            // ✅ Filmler
            if (!context.Films.Any())
            {
                var films = new List<Film>
                {
                    new Film("Inception", "Christopher Nolan", 2010, 148, Genre.SciFi),
                    new Film("Interstellar", "Christopher Nolan", 2014, 169, Genre.SciFi),
                    new Film("The Prestige", "Christopher Nolan", 2006, 130, Genre.Thriller)
                };

                context.Films.AddRange(films);
                context.SaveChanges();
            }

            // ✅ WatchHistory (demo kullanıcısı için)
            if (!context.WatchHistories.Any())
            {
                var film1 = context.Films.FirstOrDefault(f => f.Title == "Inception");
                var film2 = context.Films.FirstOrDefault(f => f.Title == "Interstellar");

                if (demoUser != null && film1 != null && film2 != null)
                {
                    var history = new List<WatchHistory>
                    {
                        new WatchHistory
                        {
                            UserId = demoUser.Id,
                            FilmId = film1.Id,
                            WatchedDate = DateTime.Now.AddDays(-7)
                        },
                        new WatchHistory
                        {
                            UserId = demoUser.Id,
                            FilmId = film2.Id,
                            WatchedDate = DateTime.Now.AddDays(-2)
                        }
                    };

                    context.WatchHistories.AddRange(history);
                    context.SaveChanges();
                }
            }
        }
    }
}
