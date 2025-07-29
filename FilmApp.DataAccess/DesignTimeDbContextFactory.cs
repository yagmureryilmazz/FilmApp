using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using FilmApp.DataAccess;  // FilmDbContext sınıfını buradan al

namespace FilmApp.DataAccess
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<FilmDbContext>
    {
        public FilmDbContext CreateDbContext(string[] args)
        {
            var connectionString = "Server=localhost,1433;Database=FilmDb;User Id=sa;Password=Passw0rd;TrustServerCertificate=True;";

            var optionsBuilder = new DbContextOptionsBuilder<FilmDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new FilmDbContext(optionsBuilder.Options);
        }
    }
}


