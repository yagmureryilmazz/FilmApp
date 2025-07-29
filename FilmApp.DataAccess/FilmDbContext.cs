using Microsoft.EntityFrameworkCore;
using FilmApp.Entities;

namespace FilmApp.DataAccess
{
    public class FilmDbContext : DbContext
    {
        public FilmDbContext(DbContextOptions<FilmDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Film> Films { get; set; }
        public DbSet<WatchHistory> WatchHistories { get; set; }
    }
}