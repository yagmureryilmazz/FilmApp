using FilmApp.Business.Interfaces;
using FilmApp.DataAccess;
using FilmApp.Entities;

namespace FilmApp.Business.Services
{
    public class FilmService : IFilmService
    {
        private readonly FilmDbContext _context;

        public FilmService(FilmDbContext context)
        {
            _context = context;
        }

        public List<Film> GetAllFilms()
        {
            return _context.Films.ToList();
        }

        public Film? GetFilmById(int id)
        {
            return _context.Films.FirstOrDefault(f => f.Id == id);
        }

        public Film AddFilm(Film film)
        {
            var existing = _context.Films
                .FirstOrDefault(f => f.Title == film.Title && f.Director == film.Director);

            if (existing != null)
                throw new Exception("Bu film zaten eklenmiş.");

            _context.Films.Add(film);
            _context.SaveChanges();
            return film;
        }

        public void DeleteFilm(int id)
        {
            var film = _context.Films.FirstOrDefault(f => f.Id == id);
            if (film == null)
                throw new Exception("Silinecek film bulunamadı.");

            _context.Films.Remove(film);
            _context.SaveChanges();
        }

        // ✅ Yeni eklenen güncelleme metodu
        public void UpdateFilm(int id, Film updatedFilm)
        {
            var film = _context.Films.FirstOrDefault(f => f.Id == id);
            if (film == null)
                throw new Exception("Güncellenecek film bulunamadı.");

            film.Title = updatedFilm.Title;
            film.Director = updatedFilm.Director;
            film.ReleaseYear = updatedFilm.ReleaseYear;
            film.Duration = updatedFilm.Duration;
            film.Genre = updatedFilm.Genre;

            _context.SaveChanges();
        }
    }
}