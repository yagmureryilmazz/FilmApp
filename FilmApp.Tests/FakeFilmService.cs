using FilmApp.Business.Interfaces;
using FilmApp.Entities;
using FilmApp.Entities.Enums;

namespace FilmApp.Tests
{
    public class FakeFilmService : IFilmService
    {
        private List<Film> _films = new List<Film>
        {
            new Film("Inception", "Nolan", 2010, 148, Genre.SciFi) { Id = 1 }
        };

        public List<Film> GetAllFilms()
        {
            return _films;
        }

        public Film GetFilmById(int id)
        {
            return _films.FirstOrDefault(f => f.Id == id)!;
        }

        public Film AddFilm(Film film)
        {
            film.Id = _films.Max(f => f.Id) + 1;
            _films.Add(film);
            return film;
        }

        public void UpdateFilm(int id, Film updatedFilm)
        {
            var film = _films.FirstOrDefault(f => f.Id == id);
            if (film != null)
            {
                film.Title = updatedFilm.Title;
                film.Director = updatedFilm.Director;
                film.ReleaseYear = updatedFilm.ReleaseYear;
                film.Duration = updatedFilm.Duration;
                film.Genre = updatedFilm.Genre;
            }
        }

        public void DeleteFilm(int id)
        {
            var film = _films.FirstOrDefault(f => f.Id == id);
            if (film != null)
                _films.Remove(film);
        }
    }
}