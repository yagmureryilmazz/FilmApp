using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FilmApp.Business.Interfaces;
using FilmApp.Entities;

namespace FilmApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FilmsController : ControllerBase
    {
        private readonly IFilmService _filmService;

        public FilmsController(IFilmService filmService)
        {
            _filmService = filmService;
        }

        /// <summary>Tüm filmleri getirir.</summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<Film>), 200)]
        public IActionResult GetFilms()
        {
            var films = _filmService.GetAllFilms();
            return Ok(films);
        }

        /// <summary>ID ile film getirir.</summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Film), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetFilm(int id)
        {
            var film = _filmService.GetFilmById(id);
            if (film == null)
                return NotFound($"ID'si {id} olan film bulunamadı.");

            return Ok(film);
        }

        /// <summary>Yeni bir film ekler.</summary>
        [HttpPost]
        [ProducesResponseType(typeof(Film), 200)]
        public IActionResult AddFilm([FromBody] Film film)
        {
            var added = _filmService.AddFilm(film);
            return Ok(added);
        }

        /// <summary>Belirli bir filmi siler.</summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult DeleteFilm(int id)
        {
            try
            {
                _filmService.DeleteFilm(id);
                return Ok($"ID'si {id} olan film silindi.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>Belirli bir filmi günceller.</summary>
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult UpdateFilm(int id, [FromBody] Film updatedFilm)
        {
            try
            {
                _filmService.UpdateFilm(id, updatedFilm);
                return Ok("Film başarıyla güncellendi.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
