using FilmApp.Entities;

namespace FilmApp.Business.Interfaces
{
    public interface IFilmService
    {
        List<Film> GetAllFilms();
        Film? GetFilmById(int id);
        Film AddFilm(Film film);
        void DeleteFilm(int id);

        // ✅ Güncelleme işlemi için yeni metot
        void UpdateFilm(int id, Film updatedFilm);
    }
}